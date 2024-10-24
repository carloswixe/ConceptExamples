namespace ConceptExamples
{
    //Delegados, Func´s, Action´s y funciones anonimas
    //func devuelve valors acepta hasta 16 params el ultimo es el valor devuelto
    //action solo "ejecuta" recibe hasta 16 params, debe ser void
    //el static no es necesario es solo por el tipo de clase que estamos usando 
    //Predicados :  Basicamente funciones de validacion, enviar una serie de parametros y dentro se valida, siempre deben tener valores boleanos
    internal static class Program
    {
        public delegate decimal Calculariva(decimal param1, decimal param2);
        public delegate decimal Calculartotal(decimal param1, decimal param2);


        static void Main(string[] args)
        {
            Calculariva Calculaiva = DEL_Calculaiva;
            Calculartotal Calculartotal = DEL_Calculatotal;
            Func<string, int> NumCadenas= FUNC_ContarCadenas;
            Action<string,string, string> Accion = ACTION_Show;
            var cadenaresultante = HacerAlgo(Calculaiva,Calculartotal, NumCadenas,Accion, 1500,1.16M);
            Action<string, string> AccionTurbo = (a, b) => Console.WriteLine(a+b);
            AccionTurbo("Accion Turbo :", cadenaresultante);
            //Predicados
            var cadenas = new List<string> { cadenaresultante, cadenaresultante, "mi cadena" };
            var predicateotherversion = new Predicate<string>(isAceptableLength);
            var predicate = new Predicate<string>(a=> a.Length<20);            
            Predicate<string> negativepredicate = x => !predicate(x);
            var cadenas2 = cadenas.FindAll(predicate);
            var cadenas3 = cadenas.FindAll(negativepredicate);

            

            //ejemplo de predicados y extensiones con cervezas
            var beers = new List<beer> {
                new beer(){Name="Pacifico Ligth",Alcohol=3.1},
                new beer(){Name="Pacifico Suave",Alcohol=3.4},
                new beer(){Name="Tecate Ligth",Alcohol=3.9},
                new beer(){Name="Amstel Ultra",Alcohol=4},
                new beer(){Name="Michelob Ultra",Alcohol=4.2},
                new beer(){Name="Pacifico Clara",Alcohol=4.4},
                new beer(){Name="Tecate",Alcohol=4.6},
                new beer(){Name="Bohemia ",Alcohol=4.7},
            };
            Console.WriteLine("\n Total Beers  \n");
            beers.ForEach(beer => { Console.WriteLine(beer.Name); });
            beers.ShowBeerThatIGetDrunk();
            beers.ShowBeerThatINotGetDrunk();
            beers.ShowBeerOnMyCondition(x=>x.Name.Contains("e"));
            Console.ReadLine();

        }

        //al agregar el this a la lista de entrada para hacer una "extension" del metodo y lo podemos usar como where, tolist, findall, etc etc etc
        static void ShowBeerThatIGetDrunk(this List<beer> beers, Predicate<beer> condicion = null) {
            condicion = condicion == null ? defaultBeerCondition : condicion;
            var evilBeers = beers.FindAll(condicion);
            Console.WriteLine("\n Evil Beers \n");
            evilBeers.ForEach(beer => {Console.WriteLine(beer.Name); });
        }
        static void ShowBeerThatINotGetDrunk(this List<beer> beers, Predicate<beer> condicion = null)
        {
            condicion = condicion == null ? defaultBeerCondition : condicion;
            //negamos el predicado para invertir el resultado
            Predicate<beer> condicionnegativa = x => !condicion(x);
            var safeBeers = beers.FindAll(condicionnegativa);
            Console.WriteLine("\n Safe Beers \n");
            safeBeers.ForEach(beer => { Console.WriteLine(beer.Name); });
        }
        static void ShowBeerOnMyCondition(this List<beer> beers, Predicate<beer> condicion)
        {
            var safeBeers = beers.FindAll(condicion);
            Console.WriteLine("\n Beers with my Custom Condition \n");
            safeBeers.ForEach(beer => { Console.WriteLine(beer.Name); });
        }

        static bool defaultBeerCondition(beer x) => x.Alcohol >= 4;
        static bool isAceptableLength(string a) => a.Length < 20;

        public static string HacerAlgo(Calculariva calculaiva, Calculartotal calculatotal, Func<string,int> contarcadenas, Action<string,string,string> imprimir, decimal subtotal, decimal tasaiva)
        {
            var mensaje = $"El subtotal es de {subtotal} \n";
            var iva = calculaiva(subtotal, tasaiva);
            mensaje += $"el iva es de {iva}\n";
            var total = calculatotal(subtotal, iva);
            mensaje += $"y el total es de {total}\n";
            var caracteresTotales = contarcadenas(mensaje);
            imprimir("Accion: "+mensaje, "TOTAL DE CARACTERES& (ActionShow): ", caracteresTotales.ToString());
            return mensaje + "TOTAL DE CARACTERES& (ActionShow): " + caracteresTotales.ToString();
        }

        public static void ACTION_Show(string cad, string cad2, string cad3) {
            Console.WriteLine(cad + cad2 + cad3+"\n");            
        }

        public static decimal DEL_Calculaiva(decimal subtotal, decimal tasaiva) {      
            
            var iva = subtotal * tasaiva;
            iva = iva-subtotal;
            return iva;
        }

        public static decimal DEL_Calculatotal(decimal subtotal, decimal iva)
        {
            var total = subtotal + iva;
            return total;
        }

        private static int FUNC_ContarCadenas(string arg)
        {
            var numerodecaracteres = arg.Length;
            return numerodecaracteres;
        }


    }

    public class beer 
    {
        public string Name { get; set; }
        public double Alcohol { get; set; }
    }
}