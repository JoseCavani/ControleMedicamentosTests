using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class ValidadorMedicamento : AbstractValidator<Medicamento>
    {

        public ValidadorMedicamento()
        {
            RuleFor(x => x.Nome).NotNull().NotEmpty();
        }

    }
}
