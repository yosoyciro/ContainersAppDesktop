﻿using ContainersDesktop.Core.Models;

namespace ContainersDesktop.Core.Contracts.Services;
public interface IMovimientosServicio
{
    Task<List<Movim>> ObtenerMovimientosObjeto(int idObjeto);
    Task<List<Movim>> ObtenerMovimientosTodos();
    Task<bool> SincronizarMovimientos(string dbDescarga, int idDispositivo);
}
