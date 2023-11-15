using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using ContainersDesktop.Helpers;
using ContainersDesktop.Logica.Services.ModelosStorage;
using ContainersDesktop.ViewModels;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;
public sealed partial class Data2MoviePage : Page
{
    public Data2MovieViewModel ViewModel
    {
        get;
    }
    
    public Data2MoviePage()
    {
        ViewModel = App.GetService<Data2MovieViewModel>();
        InitializeComponent();
        Loaded += Data2MoviePage_Loaded;        
    }

    private async void Data2MoviePage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // Verifico versión de data2movie
        var modulo = await ViewModel.AzureTableStorage.LeerTableStorage<Modulos>(nameof(Modulos), "RowKey", ViewModel.Options.Value.RowKey);
        if (modulo != null)
        {
            var subModulos = await ViewModel.AzureTableStorage.LeerTableStorage<SubModulos>(nameof(SubModulos), "PartitionKey", modulo.FirstOrDefault()!.RowKey); //"0c0b3d3a-3b35-4035-9fb5-881087f76eb4");

            var subModulosUsuarios = await ViewModel.AzureTableStorage.LeerTableStorage<SubModulosUsuarios>(nameof(SubModulosUsuarios), "PartitionKey", ViewModel.SharedViewModel.UsuarioCorreo);

            var subModulo = subModulos.FirstOrDefault(x => x.RutaSubModulos!.Contains("data2movie"));

            var existeSubModuloUsuario = subModulosUsuarios.FirstOrDefault(x => x.SubModuloId == subModulo!.RowKey);
            if (existeSubModuloUsuario == null)
            {
                await Dialogs.Error(this.XamlRoot, "No tiene configurado el acceso requerido");                
            }
            else
            {
                var pathProyecto = ViewModel.ObtenerPathProyecto();

                if (!Directory.Exists(Directory.GetParent(pathProyecto).ToString()))
                {
                    Directory.CreateDirectory(Directory.GetParent(pathProyecto).ToString());
                }

                if (string.Compare(existeSubModuloUsuario.VersionInstalada, existeSubModuloUsuario.VersionAutorizada) < 0)
                {
                    if (existeSubModuloUsuario.ActualizacionAutorizada == true)
                    {
                        var version = existeSubModuloUsuario.VersionAutorizada!.Replace(" ", "_").Replace(".", "");
                        var fileName = $"{version}_{subModulo.NombreArchivo}";
                        var downloadsFolder = Path.Combine(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName, "Downloads");
                        if (!Directory.Exists(downloadsFolder))
                        {
                            Directory.CreateDirectory(downloadsFolder);
                        }

                        try
                        {
                            await ViewModel.FileShareService.FileDownloadAsync("00downloads", fileName, downloadsFolder);

                            ZipFile.ExtractToDirectory(Path.Combine(downloadsFolder, fileName), Directory.GetParent(pathProyecto).ToString(), true);

                            if (!Debugger.IsAttached)
                            {
                                existeSubModuloUsuario.VersionInstalada = existeSubModuloUsuario.VersionAutorizada;
                                //existeSubModuloUsuario.FechaInstalacion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                                await ViewModel.AzureTableStorage.UpdateTable(existeSubModuloUsuario, nameof(SubModulosUsuarios), existeSubModuloUsuario.ETag);
                            }

                            string[] args = { ViewModel.SharedViewModel.UsuarioCorreo, ViewModel.SharedViewModel.UsuarioNombre };
                            System.Diagnostics.Process.Start(pathProyecto, args);
                        }
                        catch (Exception ex)
                        {
                            await Dialogs.Error(this.XamlRoot, ex.Message);
                        }                        
                    }
                }
                else
                {
                    string[] args = { ViewModel.SharedViewModel.UsuarioCorreo };
                    System.Diagnostics.Process.Start(pathProyecto, args);
                }                
            }            
        }
        
        Frame.GoBack();
    }
}
