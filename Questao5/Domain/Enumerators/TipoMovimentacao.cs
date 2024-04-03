using System;

namespace Questao5.Domain.Enumerators
{
    public class EnumCharValueAttribute : Attribute
    {
        public char CharValue { get; private set; }

        public EnumCharValueAttribute(char value)
        {
            CharValue = value;
        }
    }

    public enum TipoMovimentacao
    {
        [EnumCharValue('C')] Credito,
        [EnumCharValue('D')] Debito
    }
}
