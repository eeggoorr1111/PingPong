﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Narratore.DebugTools
{
    public sealed class DrawerGizmos : MonoBehaviour
    {
        private static event Action _onDraw;
        private static event Action _onPrevDraw;
        private static DrawerGizmos _instance;


        /// <param name="draw">
        /// Иногда во время отладки нужно временно отключить отрисовку гизм в некотором месте.
        /// Чтобы не стиратьне стирать полность скрипт отрисовки гизм и потом писать его заново, можно просто передать
        /// тут флаг
        /// </param>
        public static void Draw(Action callback, bool draw = true)
        {
            if (draw)
                _onDraw += callback;
        }


        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Debug.LogError($"На сцене присутствует более одного { GetType().Name }", this);
        }
        private void OnDestroy()
        {
            _instance = null;
            _onDraw = null;
            _onPrevDraw = null;
        }
        private void OnDrawGizmos()
        {
            if (_onDraw == null)
            {
                if (_onPrevDraw != null)
                    _onPrevDraw.Invoke();
            }
            else
            {
                _onDraw.Invoke();

                _onPrevDraw = null;
                foreach (var draw in _onDraw.GetInvocationList())
                    _onPrevDraw += draw as Action;

                _onDraw = null;
            }
        }
    }
}

