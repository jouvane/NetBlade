using System;
using System.Text.RegularExpressions;

namespace NetBlade.Core.ValueObject
{
    public class CpfValueObject
    {
        private string _numero;

        public CpfValueObject()
        {
        }

        public CpfValueObject(string numero)
            : this()
        {
            this.Numero = numero;
        }

        public bool IsValid
        {
            get => CpfValueObject.Validate(this.Numero);
        }

        public virtual string Numero
        {
            get => this._numero;
            set => this._numero = string.IsNullOrEmpty(value) ? string.Empty : string.Join(string.Empty, Regex.Split(value ?? string.Empty, @"[^\d]"));
        }

        public static explicit operator CpfValueObject(string value)
        {
            return new CpfValueObject(value);
        }

        public static implicit operator string(CpfValueObject value)
        {
            return value?.Numero;
        }

        public static bool Validate(string cpf)
        {
            if (cpf.Length != 11)
            {
                return false;
            }

            string digitoVerificadorCalculado,
                   digitoVerificadorInformado;

            digitoVerificadorInformado = cpf.Substring(9, 2);
            digitoVerificadorCalculado = CpfValueObject.CalculaDigitosCpf(cpf);

            return digitoVerificadorInformado == digitoVerificadorCalculado;
        }

        private static string CalculaDigitosCpf(string cpf)
        {
            string pesoCPF = "11100908070605040302";
            string parte1;
            int dv1,
                dv2,
                valor,
                i;

            parte1 = cpf.Substring(0, 9);
            valor = 0;

            for (i = 0; i <= 8; i++)
            {
                valor += Convert.ToInt32(parte1.Substring(i, 1)) * Convert.ToInt32(pesoCPF.Substring((i + 1) * 2, 2));
            }

            if ((valor % 11 == 0) | (valor % 11 == 1))
            {
                dv1 = 0;
            }
            else
            {
                dv1 = 11 - valor % 11;
            }

            parte1 += dv1.ToString();
            valor = 0;

            for (i = 0; i <= 9; i++)
            {
                valor += Convert.ToInt32(parte1.Substring(i, 1)) * Convert.ToInt32(pesoCPF.Substring(i * 2, 2));
            }

            if ((valor % 11 == 0) | (valor % 11 == 1))
            {
                dv2 = 0;
            }
            else
            {
                dv2 = 11 - valor % 11;
            }

            return Convert.ToInt32(dv1 * 10 + dv2).ToString("00");
        }

        public override string ToString()
        {
            return this.Numero;
        }
    }
}
