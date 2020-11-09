namespace servidor.InterpreteCQL.dbms
{
    public class BdUsuarios
    {
        private readonly  string id;
        private readonly  string bd;

        public string Id
        {
            get => id;
         
        }

        public string Bd
        {
            get => bd;
        
        }


        public BdUsuarios(string id, string bd)
        {
            this.id = id;
            this.bd = bd;
        }
    }
}