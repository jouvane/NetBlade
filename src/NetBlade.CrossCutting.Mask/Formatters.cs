using NetBlade.CrossCutting.Helpers;

namespace NetBlade.CrossCutting.Mask
{
    public static class Formatters
    {
        public const string MaskCEPFormat = "99.999-999";
        public const string MaskCnpjFormat = "99.999.999/9999-99";
        public const string MaskCpfFormat = "999.999.999-99";
        public const string MaskGuidFormat = "********-****-****-****-************";
        public const string MaskNumeroProcessoNetBladeFormat = "99999.999999/9999-99";

        private static readonly MaskAttribute _maskCEP = new MaskAttribute(Formatters.MaskCEPFormat);
        private static readonly MaskAttribute _maskCnpj = new MaskAttribute(Formatters.MaskCnpjFormat);
        private static readonly MaskAttribute _maskCpf = new MaskAttribute(Formatters.MaskCpfFormat);
        private static readonly MaskAttribute _maskGuid = new MaskAttribute(Formatters.MaskGuidFormat);
        private static readonly MaskAttribute _maskNumeroProcessoNetBlade = new MaskAttribute(Formatters.MaskNumeroProcessoNetBladeFormat);

        public static string CalculoDigitoVerificadorNumeroProcesso(this string numero)
        {
            string tmpValor = numero;
            string dig1;
            string dig2;

            dig1 = Formatters.CalculoDigitoVerificadorNumeroProcessoNetBlade(tmpValor);
            tmpValor += dig1;
            dig2 = Formatters.CalculoDigitoVerificadorNumeroProcessoNetBlade(tmpValor);

            return string.Concat(tmpValor, dig2);
        }

        public static string CleNetBladeaskGuid(this string codigoRequerimento)
        {
            return Formatters._maskGuid.CleanValue(codigoRequerimento);
        }

        public static string CleNetBladeaskNumeroProcessoNetBlade(this string numero)
        {
            return Formatters._maskNumeroProcessoNetBlade.CleanValue(numero ?? string.Empty);
        }

        public static string MaskCEP(this string cep)
        {
            return Formatters._maskCEP.Format(StringHelper.OnlyNumbers(cep ?? string.Empty));
        }

        public static string MaskCnpj(this string cnpj)
        {
            return Formatters._maskCnpj.Format(StringHelper.OnlyNumbers(cnpj ?? string.Empty));
        }

        public static string MaskCpf(this string cpf)
        {
            return Formatters._maskCpf.Format(StringHelper.OnlyNumbers(cpf ?? string.Empty));
        }

        public static string MaskCpfCnpj(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            if (input.Length == 14)
            {
                return input.MaskCnpj();
            }

            if (input.Length == 11)
            {
                return input.MaskCpf();
            }

            return input;
        }

        public static string MaskGuid(this string codigoRequerimento)
        {
            return Formatters._maskGuid.Format((codigoRequerimento ?? string.Empty).CleNetBladeaskGuid() ?? string.Empty);
        }

        public static string MaskNumeroProcessoNetBlade(this string numero)
        {
            return Formatters._maskNumeroProcessoNetBlade.Format(StringHelper.OnlyNumbers(numero ?? string.Empty));
        }

        public static string MaskNumeroProcessoNetBlade(object numeroProcesso, object anoProcesso)
        {
            return string.Format("{0:000\\.000}/{1:0000}", int.Parse(numeroProcesso.ToString().Replace(".", string.Empty)), anoProcesso);
        }

        public static string ObterAnoProcessoNetBlade(string numeroAno)
        {
            numeroAno = (numeroAno ?? string.Empty).Replace("_", string.Empty);
            if (!string.IsNullOrEmpty(numeroAno))
            {
                if (numeroAno.Length == 12)
                {
                    return numeroAno.Replace(".", string.Empty).Substring(8, 4);
                }

                if (numeroAno.Length == 10)
                {
                    return numeroAno.Substring(6, 4);
                }

                if (numeroAno.Length == 4)
                {
                    return numeroAno;
                }

                if (numeroAno.Contains('/'))
                {
                    string parseResult = numeroAno.Replace(".", string.Empty);
                    return parseResult.Substring(parseResult.IndexOf('/') + 1, 4);
                }
            }

            return null;
        }

        public static string ObterNumeroSemAno(string numeroAno)
        {
            numeroAno = (numeroAno ?? string.Empty).Replace("_", string.Empty);
            if (!string.IsNullOrEmpty(numeroAno))
            {
                if (numeroAno.Length == 12)
                {
                    return numeroAno.Replace(".", string.Empty).Substring(0, 6);
                }

                if (numeroAno.Length == 10)
                {
                    return numeroAno.Substring(0, 6);
                }

                if (numeroAno.Length == 6)
                {
                    return numeroAno;
                }

                if (numeroAno.Contains('/'))
                {
                    string parseResult = numeroAno.Replace(".", string.Empty);
                    return parseResult.Substring(0, parseResult.IndexOf('/'));
                }
            }

            return null;
        }

        public static string OnlyNumbers(this string strNumber)
        {
            return StringHelper.OnlyNumbers(strNumber);
        }

        private static string CalculoDigitoVerificadorNumeroProcessoNetBlade(string numero)
        {
            int acm = 0;
            string dv;
            for (int i = 2; i <= numero.Length + 1; i++)
            {
                acm += int.Parse(numero.Substring(numero.Length + 1 - i, 1)) * i;
            }

            int resto = acm % 11;
            dv = (11 - resto).ToString();
            return dv.Substring(dv.Length - 1, 1);
        }
    }
}
