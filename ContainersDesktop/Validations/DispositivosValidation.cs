using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Services;

namespace ContainersDesktop.Validations;
public class DispositivosValidation
{
    public class ContainerLocalValidationAttribute : ValidationAttribute
    {
        private readonly IAsyncRepository<Dispositivo> _dispositivosRepo;
        private bool containerIsValid = true;

        public ContainerLocalValidationAttribute() : base("El container está asociado a otro dispositivo")
        {
            _dispositivosRepo = App.GetService<IAsyncRepository<Dispositivo>>();
        }

        private async Task ContainerAsociadoDispositivo(string container)
        {
            containerIsValid = true;
            var regex = @"^[a-z][a-z][a-z][0-9][0-9][0-9]";
            if (!Regex.IsMatch(container, regex))
            {
                return;
            }
            var dispositivos = await _dispositivosRepo.GetAsync();

            foreach (var item in dispositivos)
            {
                if (item.DISPOSITIVOS_CONTAINER == container && item.Estado == "A")
                {
                    containerIsValid = false;
                    break;
                }
            }                       
        }

        public override bool IsValid(object value)
        {                  
            ContainerAsociadoDispositivo((string)value).Wait();
            return containerIsValid;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, containerIsValid);
        }
    }

    public class ContainerAzureValidationAttribute : ValidationAttribute
    {
        private readonly AzureStorageManagement _azureStorageManagement;
        private bool containerIsValid = true;

        public ContainerAzureValidationAttribute() : base("El container no existe en la nube")
        {
            _azureStorageManagement = App.GetService<AzureStorageManagement>();
        }

        private bool ContainerExiste(string container)
        {
            containerIsValid = true;
            var regex = @"^[a-z][a-z][a-z][0-9][0-9][0-9]";
            if (!Regex.IsMatch(container, regex))
            {
                return true;
            }

            return _azureStorageManagement.ConsultarDispositivo(container);
        }

        public override bool IsValid(object value)
        {
            containerIsValid = ContainerExiste((string)value);
            return containerIsValid;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, containerIsValid);
        }
    }
}
