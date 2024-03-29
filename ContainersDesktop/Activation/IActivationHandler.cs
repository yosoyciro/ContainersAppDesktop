﻿using System.Threading.Tasks;

namespace ContainersDesktop.Activation;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
