using System;
using System.Collections.Generic;
using UnityEngine;

namespace Untils
{
    public class PoolObject<T>
    {
        private List<T> _poolAllObj = new List<T>();
        private List<T> _poolActiveObj = new();
        private Queue<T> _poolDisabledObj = new Queue<T>();

        private Func<T> _generationObj;
        private Action<T> _returnInActive;
        private Action<T> _returnActive;

        public IReadOnlyList<T> ActiveObjects => _poolActiveObj;

        public PoolObject(Func<T> generationObj, Action<T> returnInActive, Action<T> returnActive, int countObj)
        {
            _generationObj += generationObj;
            _returnInActive += returnInActive;
            _returnActive += returnActive;

            GenerationStartingObj(countObj);
        }

        private void GenerationStartingObj(int countObjs)
        {
            for (var i = 0; i < countObjs; i++)
            {
                var obj = _generationObj();
                _poolAllObj.Add(obj);
                ReturnInActive(obj);
            }
        }

        public void ReturnInActive(T obj)
        {
            _returnInActive(obj);
            _poolActiveObj.Remove(obj);
            _poolDisabledObj.Enqueue(obj);
        }

        public void ReturnActive(int countObjs)
        {
            for (var i = 0; i < countObjs; i++)
            {
                var obj = _poolDisabledObj.Count > 0 ? _poolDisabledObj.Dequeue() : _generationObj();
                _returnActive(obj);
                _poolActiveObj.Add(obj);
            }
        }
    }
}
