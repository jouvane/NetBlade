using System;

namespace NetBlade.CrossCutting.Mask
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MaskAttribute : Attribute
    {
        public MaskAttribute(string maskTemplate)
            : this(new[] { maskTemplate }, true, true)
        {
        }

        public MaskAttribute(string[] maskTemplate)
            : this(maskTemplate, true, true)
        {
        }

        public MaskAttribute(string[] maskTemplate, bool clearMaskSubmit, bool formatInRender)
        {
            this.ClearMaskSubmit = clearMaskSubmit;
            this.FormatInRender = formatInRender;
            this.MaskTemplate = maskTemplate;
            this.ProviderMask = MaskAttribute.ProviderMaskType != null ? (IMask)Activator.CreateInstance(MaskAttribute.ProviderMaskType) : new JqueryMask(this.MaskTemplate);
        }

        public static Type ProviderMaskType { get; set; }

        public bool ClearMaskSubmit { get; }

        public bool FormatInRender { get; }

        public string[] MaskTemplate { get; }

        public IMask ProviderMask { get; }

        public string CleanValue(string value)
        {
            return this.ProviderMask.CleanValue(value);
        }

        public string Format(string value)
        {
            return this.ProviderMask.Format(value);
        }
    }
}
