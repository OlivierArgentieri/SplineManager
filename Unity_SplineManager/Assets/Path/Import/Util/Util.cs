using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Unity_Framework.Scripts.Import.Util
{
    public abstract class Util
    {

        #region f/p /const

        private const BindingFlags reflectionFlags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        #endregion
        
        /// <summary>
        /// Simple Clamp Rotation Algorithm
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_maxValue"></param>
        /// <param name="_minValue"></param>
        /// <returns></returns>
        public static float ClampRotation(float _value, float _maxValue, float _minValue)
        {
            float _toReturn = _value;

            if (_toReturn > _maxValue)
                _toReturn = _maxValue;
        
            else if (_toReturn < _minValue)
                _toReturn = _minValue;

            return _toReturn % 360;
        }

        /// <summary>
        /// Convert Coordinate to Ray
        /// </summary>
        /// <param name="_camera"></param>
        /// <param name="_v2"></param>
        /// <param name="_distance"></param>
        /// <returns></returns>
        public static Ray ConvertCordToRay(Camera _camera, Vector2 _v2, float _distance) => _camera.ScreenPointToRay(new Vector3(_v2.x, _v2.y, _distance));
        
        /// <summary>
        /// Delayed Callaback => Corountine 
        /// </summary>
        /// <param name="_time"></param>
        /// <param name="_callback"></param>
        /// <returns></returns>
        public static IEnumerator DelayedCallback(float _time, Action _callback)
        {
            yield return new WaitForSeconds(_time);
            _callback?.Invoke();
        }
        
        

        /// <summary>
        /// Get field by reflection
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_target"></param>
        /// <param name="_flags"></param>
        /// <returns> fieldInfo object</returns>
        public static FieldInfo GetField(string _name, object _target, BindingFlags _flags = reflectionFlags) => _target.GetType().GetField(_name, _flags);
    }
}