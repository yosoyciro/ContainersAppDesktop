﻿using System.Threading.Tasks;

namespace ContainersDesktop.Logica.Services;

public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);
    Task DeleteSettingAsync(string key);
}
