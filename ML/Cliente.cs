namespace ML
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int NumeroCuenta { get; set; }
        public int Nip { get; set; }
        public  int Saldo { get; set; }
        public int saldoActual { get; set; }
        public List<Object> Clientes { get; set; }
        public ML.DetalleCuenta DetalleCuenta { get; set; }
    }
}