﻿using System;
using authService.Configuration;

namespace authService.Settings
{
    public class Application : Settings
    {
        public Connections Connections { get; set; }
    }
}
