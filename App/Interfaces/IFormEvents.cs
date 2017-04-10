﻿using System;

namespace App.Interfaces
{
    public interface IFormEvents
    {
        event EventHandler FormClosed;
        event EventHandler FormClosing;
        void EmitFormClosing();
        void EmitFormClosed();
    }
}