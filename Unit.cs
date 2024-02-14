﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning
{
    /// <summary>
    /// Установка
    /// </summary>
    internal class Unit
    {
        public int Id {  get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        private Factory _factory;
    }
}
