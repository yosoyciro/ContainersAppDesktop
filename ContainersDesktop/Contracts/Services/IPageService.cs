using System;

namespace ContainersDesktop.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);
}
