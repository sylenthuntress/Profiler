﻿using System;
using BepInEx.Logging;

namespace Profiler.Utils;

internal static class LogManager {
    private static ManualLogSource _logSource;

    internal static void Init(ManualLogSource logSource) {
        _logSource = logSource;
        Info("{}-{} has loaded!", Profiler.PluginGUID, Profiler.PluginVersion);
    }

    internal static void Debug(string message) => Debug(message, null);
    internal static void Debug(string message, params object[] data) => _logSource.LogDebug(Write(message, data));
    
    internal static void Error(string message) => Error(message, null);
    internal static void Error(string message, params object[] data) => _logSource.LogError(Write(message, data));
    
    internal static void Fatal(string message) => Fatal(message, null);
    internal static void Fatal(string message, params object[] data) => _logSource.LogFatal(Write(message, data));
    
    internal static void Info(string message) => Info(message, null);
    internal static void Info(string message, params object[] data) => _logSource.LogInfo(Write(message, data));
    
    internal static void Message(string message) => Message(message, null);
    internal static void Message(string message, params object[] data) => _logSource.LogMessage(Write(message, data));

    internal static void Warning(string message) => Warning(message, null);
    internal static void Warning(string message, params object[] data) => _logSource.LogWarning(Write(message, data));

    private static string Write(string message, params object[] data) {
        if (data == null) {
            return message;
        }

        int arrayIndex = 0;
        foreach (object obj in data) {
            int index = message.IndexOf("{" + arrayIndex + "}", StringComparison.Ordinal);
            int term = ("{" + arrayIndex + "}").Length;
            if (index == -1) {
                index = message.IndexOf("{}", StringComparison.Ordinal);
                term = 2;
                if (index == -1) {
                    break;
                }
            }

            message = message[..index] + obj + message[(index + term)..];
            arrayIndex++;
        }

        return message;
    }
}
