using System;
using System.Collections.Generic;
using System.Linq;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.CoreFolder.LevelFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class CircleFillService : IRestarted
	{
		private readonly IReadOnlyCollection<FalCircleReader> _falCircleReaders;
		private readonly int _maxNumberCircleColumn;
		private readonly CircleDisappearanceService _circleDisappearanceService;
		private readonly GameStateMachine _gameStateMachine;

		private Dictionary<FalCircleReader, CircleObject[]> _lines;
		private readonly OvercrowdingTrigger _overcrowdingTrigger;

		public event Action Losed;
		public event Action<CircleObject[]> CircleCollected;

		public CircleFillService(LevelObject levelObject, CircleDisappearanceService circleDisappearanceService, GameStateMachine gameStateMachine)
		{
			_circleDisappearanceService = circleDisappearanceService;
			_gameStateMachine = gameStateMachine;
			_falCircleReaders = levelObject.FalCircleReaders;
			_overcrowdingTrigger = levelObject.OvercrowdingTrigger;
			_maxNumberCircleColumn = levelObject.NumberCircleColumn;

			CreatLinesData();
			Subscribe();
		}

		private void CreatLinesData()
		{
			_lines = new Dictionary<FalCircleReader, CircleObject[]>(_falCircleReaders.Count);

			foreach(FalCircleReader falCircle in _falCircleReaders)
				_lines.Add(falCircle, new CircleObject[_maxNumberCircleColumn]);
		}

		private void Subscribe()
		{
			foreach(FalCircleReader falCircleReader in _falCircleReaders)
				falCircleReader.OnCircleCollected += OnCircleCollected;

			_overcrowdingTrigger.OnCircleCollected += OnOvercrowdingTriggerCircleCollected;
		}

		private void OnOvercrowdingTriggerCircleCollected(CircleObject arg1)
		{
			//Не Было написано как действовать для непревышения размерности башни больше 3
			Lose();
		}

		private void OnCircleCollected(CircleObject circleObject, FalCircleReader falCircleReader)
		{
			int index = -1;

			CircleObject[] line = _lines[falCircleReader];

			if(line.Any(p => p == circleObject))
				return;

			for( int i = 0; i < line.Length; i++ )
			{
				if(line[i] != null)
					continue;

				line[i] = circleObject;
				index = i;
				break;
			}

			if(index == -1)
				throw new Exception("Game field settings error");

			bool result = ValidateLines(falCircleReader, index);

			if(index == _maxNumberCircleColumn - 1 && !result)
				CheckLosses();
		}

		private void CheckLosses()
		{
			bool result = _lines.All(p => p.Value[^1] != null);

			if(!result)
				return;

			Lose();
		}

		private void Lose()
		{
			_gameStateMachine.TrySetState(GameState.EndGame);
			Losed?.Invoke();
		}

		private bool ValidateLines(FalCircleReader falCircleReader, int index)
		{
			CircleObject[] main = _lines[falCircleReader];

			bool validateVertical = ValidateVertical(main);
			bool validateHorizontal = ValidateHorizontal(index, main);
			bool validateDiagonal = ValidateDiagonal(falCircleReader, index, out bool lineA, out bool lineB);

			if(validateHorizontal)
				CollectHorizontalLine(index);

			if(validateVertical)
				CollectVerticalLine(falCircleReader);

			if(validateDiagonal)
				CollectDiagonalLine(falCircleReader, index, lineA, lineB);

			LineAlignment();

			return validateVertical || validateHorizontal || validateDiagonal;
		}


		private bool ValidateHorizontal(int index, CircleObject[] main)
		{
			bool isValidate = true;

			if(main[index] == null)
				return false;

			string id = main[index].ID;

			foreach(KeyValuePair<FalCircleReader, CircleObject[]> line in _lines)
			{
				isValidate = line.Value[index] != null && line.Value[index].ID.Equals(id);

				if(!isValidate)
					break;
			}

			if(isValidate)
				Debug.Log("ValidateHorizontal");

			return isValidate;
		}

		private bool ValidateVertical(CircleObject[] main)
		{
			bool isValidate = true;

			if(main[0] == null)
				return false;

			string id = main[0].ID;

			for( int i = 1; i < _maxNumberCircleColumn; i++ )
			{
				isValidate = main[i] != null && main[i].ID.Equals(id);

				if(!isValidate)
					break;
			}

			if(isValidate)
				Debug.Log("ValidateVertical");

			return isValidate;
		}

		private bool ValidateDiagonal(FalCircleReader falCircleReader, int index, out bool lineA, out bool lineB)
		{
			FalCircleReader[] keys = _lines.Keys.ToArray();

			string id = _lines[falCircleReader][index].ID;

			lineA = true;
			lineB = true;

			int lengthA = 0;
			int lengthB = 0;

			for( int i = 0; i < keys.Length; i++ )
			{
				var line = _lines[keys[i]];
				
				int elementIndexA = _maxNumberCircleColumn - 1 - i;

				if(elementIndexA < 0 || elementIndexA >= _maxNumberCircleColumn)
					continue;

				CircleObject objectA = line[elementIndexA];
				lineA = objectA != null && objectA.ID.Equals(id);

				if(lineA)
					lengthA++;

				if(!lineA)
					break;
			}

			for( int i = 0; i < keys.Length; i++ )
			{
				var line = _lines[keys[i]];
				
				int elementIndexB = i;

				if(elementIndexB < 0 || elementIndexB >= _maxNumberCircleColumn)
					continue;

				CircleObject objectB = line[elementIndexB];
				lineB = objectB != null && objectB.ID.Equals(id);

				if(lineB)
					lengthB++;

				if(!lineB)
					break;
			}

			lineA = lineA && lengthA == _maxNumberCircleColumn;
			lineB = lineB && lengthB == _maxNumberCircleColumn;
			bool isValidate = lineA || lineB;

			return isValidate;
		}

		private void CollectVerticalLine(FalCircleReader falCircleReader)
		{
			List<CircleObject> circleObjects = new(3);
			CircleObject[] main = _lines[falCircleReader];

			for( int i = 0; i < main.Length; i++ )
			{
				CircleObject circleObject = main[i];

				if(circleObject == null)
					continue;

				circleObjects.Add(circleObject);
				main[i] = null;
			}
			CircleCollected?.Invoke(circleObjects.ToArray());
			_circleDisappearanceService.Disappearance(circleObjects.ToArray());
		}

		private void CollectHorizontalLine(int index)
		{
			List<CircleObject> circleObjects = new List<CircleObject>(3);

			foreach(KeyValuePair<FalCircleReader, CircleObject[]> line in _lines)
			{
				CircleObject circle = line.Value[index];

				if(circle == null)
					continue;

				circleObjects.Add(circle);
				line.Value[index] = null;
			}

			CircleCollected?.Invoke(circleObjects.ToArray());
			_circleDisappearanceService.Disappearance(circleObjects.ToArray());
		}

		private void CollectDiagonalLine(FalCircleReader falCircleReader, int index, bool lineA, bool lineB)
		{
			List<CircleObject> circleObjects = new List<CircleObject>(3);

			FalCircleReader[] keys = _lines.Keys.ToArray();

			for( int i = 0; i < keys.Length; i++ )
			{
				CircleObject[] objects = _lines[keys[i]];
				
				int elementIndexA = _maxNumberCircleColumn - 1 - i;
				
				if(elementIndexA < 0 || elementIndexA >= _maxNumberCircleColumn)
					continue;
				
				if(lineA)
				{
					CircleObject target = objects[elementIndexA];

					if(target != null)
					{
						circleObjects.Add(target);
						objects[elementIndexA] = null;
					}
				}
			}

			for( int i = 0; i < keys.Length; i++ )
			{
				CircleObject[] objects = _lines[keys[i]];
				
				int elementIndexB = i;
				
				if(elementIndexB < 0 || elementIndexB >= _maxNumberCircleColumn)
					continue;
				
				if(lineB)
				{
					CircleObject target = objects[elementIndexB];

					if(target != null)
					{
						circleObjects.Add(target);
						objects[elementIndexB] = null;
					}
				}
			}
			
			if(circleObjects.Count > 0)
				Debug.Log("CollectHorizontalLine");

			CircleCollected?.Invoke(circleObjects.ToArray());
			_circleDisappearanceService.Disappearance(circleObjects.ToArray());
		}

		private void LineAlignment()
		{
			Dictionary<FalCircleReader, List<int>> moved = DefineOffset();
			CheckEffectsOffset(moved);
		}
		
		private void CheckEffectsOffset(Dictionary<FalCircleReader, List<int>> moved)
		{
			foreach(KeyValuePair<FalCircleReader, List<int>> valuePair in moved)
			{
				foreach(int ids in valuePair.Value)
					ValidateLines(valuePair.Key, ids);
			}
		}

		private Dictionary<FalCircleReader, List<int>> DefineOffset()
		{
			Dictionary<FalCircleReader, List<int>> moved = new Dictionary<FalCircleReader, List<int>>();
			FalCircleReader[] keys = _lines.Keys.ToArray();
			
			foreach(FalCircleReader key in keys)
			{
				CircleObject[] circles = _lines[key];

				for( int i = 0; i < circles.Length; i++ )
				{
					if(circles[i] != null)
						continue;

					for( int j = i + 1; j < circles.Length; j++ )
					{
						if(circles[j] == null)
							continue;

						circles[i] = circles[j];
						circles[j] = null;

						if(!moved.ContainsKey(key))
							moved.Add(key, new List<int>());

						moved[key].Add(i);

						break;
					}
				}
			}
			return moved;
		}

		private void Unsubscribe()
		{
			foreach(FalCircleReader falCircleReader in _falCircleReaders)
				falCircleReader.OnCircleCollected -= OnCircleCollected;
		}

		public void Restart()
		{
			FalCircleReader[] keys = _lines.Keys.ToArray();

			foreach(FalCircleReader key in keys)
				_lines[key] = new CircleObject[_maxNumberCircleColumn];
		}

	}
}