using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.ScriptsFolder.DataFolder.CirclesDataFolder
{
    [CreateAssetMenu(menuName = Constants.Circle.CircleConfigurationCaseMenuPath, fileName = Constants.Circle.CircleConfigurationCaseBaseName, order = 0)]
    public class CircleConfiguration : ScriptableObject
    {
        [SerializeField] private CircleConfigurationCase[] _configurations;

        private Dictionary<string, CircleConfigurationData> _configurationsData;
        
        public IReadOnlyDictionary<string, CircleConfigurationData> GetConfigurations()
        {
            if(_configurationsData == null)
                CreateConfigurationsData();

            return _configurationsData;
        }
        
        private void CreateConfigurationsData()
        {
            _configurationsData = new Dictionary<string, CircleConfigurationData>(_configurations.Length);
            
            foreach(CircleConfigurationCase config in _configurations)
                _configurationsData.Add(config.ID, new CircleConfigurationData(config, config.ID));
        }
    }

    public struct CircleConfigurationData
    {
        public readonly string ID;
        public readonly Sprite Sprite;
        public readonly int Price;
        public readonly CircleVisualDisappearanceInfo DisappearanceInfo;

        public CircleConfigurationData(CircleConfigurationCase configuration, string id)
        {
            ID = id;
            Price = configuration.Price;
            Sprite = configuration.Sprite;
            DisappearanceInfo = configuration.DisappearanceInfo;
        }

    }
    
    [Serializable]
    public struct CircleConfigurationCase
    {
        public string ID;
        public Sprite Sprite;
        public int Price;
        public CircleVisualDisappearanceInfo DisappearanceInfo;
    }
    
    [Serializable]
    public struct CircleVisualDisappearanceInfo
    {
        public Color ParticleColor;
        public float Time;
        public Vector3 Angle;
    }
}
