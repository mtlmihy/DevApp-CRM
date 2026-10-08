using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;


namespace DataverseLoginPasswordSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var connStr =
                $"AuthType=OAuth;" +
                $"Url=https://org9b2b3736.crm12.dynamics.com;" + // <---- Il n'y a que l'url à changer
                $"AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;" +
                $"RedirectUri=http://localhost;" +
                $"LoginPrompt=Auto;";

            try
            {
                using (ServiceClient serviceClient = new ServiceClient(connStr))
                {
                    if (serviceClient?.IsReady == true)
                    {
                        var account = new Entity("account");
                        account["name"] = "Mon nouveau compte";
                        account["telephone1"] = "0123456789";
                        account["emailaddress1"] = "contact@exemple.com";
                        account["websiteurl"] = "https://www.exemple.com";

                        Guid accountId = serviceClient.Create(account);
                        
                        Console.WriteLine("Connexion Dataverse OK !");
                        // Exemple : récupération de 10 comptes
                        var query = new Microsoft.Xrm.Sdk.Query.QueryExpression("account")
                        {
                            TopCount = 10,
                            ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("name", "accountnumber")
                        };
                        var result = serviceClient.RetrieveMultiple(query);
                        foreach (var entity in result.Entities)
                        {
                            var name = entity.GetAttributeValue<string>("name");
                            var numéro = entity.GetAttributeValue<string>("accountnumber");
                            Console.WriteLine($"Nom: {name} - Numéro: {numéro}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Echec connexion Dataverse. Vérifie tes identifiants et droits.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }
    }
}
