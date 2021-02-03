using System;
using System.Text.RegularExpressions;

namespace NetBlade.Core.ValueObject
{
    public class CnpjValueObject
    {
        private string _numero;

        public CnpjValueObject()
        {
        }

        public CnpjValueObject(string numero)
            : this()
        {
            this.Numero = numero;
        }

        public bool IsValid
        {
            get => CnpjValueObject.Validate(this.Numero);
        }

        public virtual string Numero
        {
            get => this._numero;
            set => this._numero = string.IsNullOrEmpty(value) ? string.Empty : string.Join(string.Empty, Regex.Split(value ?? string.Empty, @"[^\d]"));
        }

        public static explicit operator CnpjValueObject(string value)
        {
            return new CnpjValueObject(value);
        }

        public static implicit operator string(CnpjValueObject value)
        {
            return value?.Numero;
        }

        public static bool Validate(string cnpj)
        {
            if (cnpj.Length != 14)
            {
                return false;
            }

            string digitoVerificadorCalculado,
                   digitoVerificadorInformado;
            digitoVerificadorInformado = cnpj.Substring(12, 2);
            digitoVerificadorCalculado = CnpjValueObject.CalculaDigitosCnpj(cnpj);

            return digitoVerificadorInformado == digitoVerificadorCalculado;
        }

        private static string CalculaDigitosCnpj(string cnpj)
        {
            const string pesoCnpj = "6543298765432";
            string parte1;
            int dv1,
                dv2,
                valor,
                i;

            parte1 = cnpj.Substring(0, 12);
            valor = 0;

            for (i = 0; i <= 11; i++)
            {
                valor += Convert.ToInt32(parte1.Substring(i, 1)) * Convert.ToInt32(pesoCnpj.Substring(i + 1, 1));
            }

            if ((valor % 11 == 0) | (valor % 11 == 1))
            {
                dv1 = 0;
            }
            else
            {
                dv1 = 11 - valor % 11;
            }

            valor = 0;

            for (i = 0; i <= 11; i++)
            {
                valor += Convert.ToInt32(parte1.Substring(i, 1)) * Convert.ToInt32(pesoCnpj.Substring(i, 1));
            }

            valor += dv1 * 2;

            if ((valor % 11 == 0) | (valor % 11 == 1))
            {
                dv2 = 0;
            }
            else
            {
                dv2 = 11 - valor % 11;
            }

            return dv1 + dv2.ToString();
        }

        public override string ToString()
        {
            return this.Numero;
        }
    }
}
