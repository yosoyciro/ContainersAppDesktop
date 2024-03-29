﻿using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.Dominio.Models.Base;
public abstract class BaseEntity
{
    [Key]
    public int ID
    {
        get; set;
    }   
}
