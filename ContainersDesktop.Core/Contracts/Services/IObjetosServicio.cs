﻿using ContainersDesktop.Core.Models;

namespace ContainersDesktop.Core.Contracts.Services;
public interface IObjetosServicio
{
    Task<List<Objetos>> ObtenerObjetos();
    Task<Objetos> ObtenerObjetoPorId(int id);
    Task<bool> CrearObjeto(Objetos objeto);
    Task<bool> ActualizarObjeto(Objetos objeto);
    Task<bool> BorrarObjeto(int id);
}