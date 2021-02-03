namespace NetBlade.Core.Security
{
    public class EncryptedInt
    {
        public EncryptedInt(int value)
        {
            this.Value = value;
        }

        public EncryptedInt(string encrypted)
        {
            this.Value = int.Parse(DESCryptoService.Desencrypt("EncryptedInt", encrypted));
        }

        public int Value { get; set; }

        public static explicit operator EncryptedInt(int value)
        {
            return new EncryptedInt(value);
        }

        public static implicit operator int(EncryptedInt value)
        {
            return value?.Value ?? 0;
        }

        public override string ToString()
        {
            return DESCryptoService.Encrypt("EncryptedInt", this.Value.ToString());
        }
    }
}
