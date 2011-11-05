﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace EnsemPro
{
    public class InputBuffer
    {
        List<InputState> items = new List<InputState>(100);

        public void Add(InputState s)
        {
            items.Add(s);
            CurrentPosition = s.position;
            VolumeChange = s.key;
            //Console.WriteLine("KEYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY " + s.key);
        }

        public Keys VolumeChange
        {
            get;
            private set;
        }

        public Vector2 CurrentPosition
        {
            get;
            private set;
        }

        public void Clear()
        {
            items.Clear();
        }

        public InputState this[int index]
        {
            get { return items[index]; }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public IEnumerator<InputState> GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}