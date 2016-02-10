namespace Jasily.Framework.ConsoleEngine.Entities
{
    public struct OperationResult
    {
        public string Error { get; }

        public OperationResult(string error)
        {
            this.Error = error;
        }

        public bool HasError => this.Error != null;

        public static implicit operator string(OperationResult r) => r.Error ?? string.Empty;

        public static implicit operator OperationResult(string r) => new OperationResult(r);
    }
}