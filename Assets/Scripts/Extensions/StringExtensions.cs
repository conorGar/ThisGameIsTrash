﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string value)
    {
        if (value != null) {
            for (int i = 0; i < value.Length; i++) {
                if (!char.IsWhiteSpace(value[i])) {
                    return false;
                }
            }
        }
        return true;
    }
}
