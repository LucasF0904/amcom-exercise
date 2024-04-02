using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        private const double TaxaDeSaque = 3.50;

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0.0;
        }

        public ContaBancaria(int numero, string titular, double depositoInicial) : this(numero, titular)
        {
            Deposito(depositoInicial);
        }

        public void Deposito(double quantia)
        {
            if (quantia <= 0)
            {
                throw new ArgumentException("Valor de depósito deve ser positivo.");
            }
            Saldo += quantia;
        }

        public void Saque(double quantia)
        {
            if (quantia <= 0)
            {
                throw new ArgumentException("Valor de saque deve ser positivo.");
            }
            if (Saldo - quantia - TaxaDeSaque < 0)
            {
                throw new InvalidOperationException("Saldo insuficiente para realizar o saque e cobrir a taxa de saque.");
            }
            Saldo -= (quantia + TaxaDeSaque);
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}
