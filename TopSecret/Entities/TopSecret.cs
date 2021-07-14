using System;

namespace TopSecretLibrary
{
    public class TopSecret
    {
        private Position _posA;
        private Position _posB;
        private Position _posC;

        public TopSecret(Position posA, Position posB, Position posC) {
            _posA = posA;
            _posB = posB;
            _posC = posC;
        }

        /// <summary>
        /// Función que se encarga de triangular la posición del objetivo dadas las coordenadas de los 3 satelites y las distancias de cada uno de ellos al objetivo
        /// </summary>
        /// <param name="dist1">Distancia del satelite 1 al objetivo</param>
        /// <param name="dist2">Distancia del satelite 2 al objetivo</param>
        /// <param name="dist3">Distancia del satelite 3 al objetivo</param>
        /// <returns>Posicion del objetivo. Objeto del tipo Position (X,Y). Devuleve NULL si no puedo encontrar la posición</returns>
        public Position GetLocation(double dist1, double dist2, double dist3) {
            bool flgOK;

            Position posAB1 = new Position(0,0);
            Position posAB2 = new Position(0,0);

            flgOK = CalcVertexC(dist1, dist2, _posA, _posB, ref posAB1, ref posAB2); //Calculo las dos coordenadas donde se tocan los satelites 1 y 2

            if (flgOK) {
                Position posAC1 = new Position(0, 0);
                Position posAC2 = new Position(0, 0);

                flgOK = CalcVertexC(dist1, dist3, _posA, _posC, ref posAC1, ref posAC2); //Calculo las dos coordenadas donde se tocan los satelites 1 y 3 

                if (flgOK) { 
                    Position Pos = null;

                    //Verifico si hay coincidencia de puntos
                    if ((posAB1.X == posAC1.X && posAB1.Y == posAC1.Y) || (posAB1.X == posAC2.X && posAB1.Y == posAC2.Y))
                        Pos = posAB1;
                    else if ((posAB2.X == posAC1.X && posAB2.Y == posAC1.Y) || (posAB2.X == posAC2.X && posAB2.Y == posAC2.Y))
                        Pos = posAB2;

                    return Pos;
                }
            }

            return null;
        }

        /// <summary>
        /// Función trigonometrica que calcula el vertice C de un triangulo, dados los vertices A y B como dato
        /// </summary>
        /// <param name="distA">Distancia entre el punto A y el C</param>
        /// <param name="distB">Distancia entre el punto B y el C</param>
        /// <param name="posA">Coordenadas del vertice A</param>
        /// <param name="posB">Coordenadas del vertice B</param>
        /// <param name="pos1">Primer punto solución</param>
        /// <param name="pos2">Segundo punto solución</param>
        /// <returns>Verdadero si pudo encontrar solución. Falso si no la hay</returns>
        private bool CalcVertexC(double distA, double distB, Position posA, Position posB, ref Position pos1, ref Position pos2)
        {
            try{
                double a, b, c;

                pos1.X = 0;
                pos1.Y = 0;
                pos2.X = 0;
                pos2.Y = 0;

                double[] eqA = new double[3];
                double[] eqB = new double[3];
                double[] eqDif = new double[3];

                eqA[0] = 2 * (posA.X * -1);
                eqA[1] = 2 * (posA.Y * -1);
                eqA[2] = Round((posA.X * posA.X) + (posA.Y * posA.Y) + (distA * distA *(-1)));

                eqB[0] = 2 * (posB.X * -1);
                eqB[1] = 2 * (posB.Y * -1);
                eqB[2] = Round((posB.X * posB.X) + (posB.Y * posB.Y) + (distB * distB * (-1)));

                eqDif[0] = eqA[0] - eqB[0]; //x
                eqDif[1] = eqA[1] - eqB[1]; //y
                eqDif[2] = eqA[2] - eqB[2]; //termino independiente

                if (eqDif[1] == 0)
                {
                    pos1.X = (eqDif[2]*(-1)) / eqDif[0];
                    
                    a = 1;
                    b = eqB[1];
                    c = (eqB[2]) + (eqB[0] * pos1.X) + (pos1.X * pos1.X);

                    pos1.Y = Round(((b * (-1)) + Math.Sqrt((b * b) - (4 * a * c))) / (2 * a));
                    pos2.Y = Round(((b * (-1)) - Math.Sqrt((b * b) - (4 * a * c))) / (2 * a));

                    pos1.X = Round(pos1.X);
                    pos2.X = pos1.X;
                }
                else
                {
                    double term1Aux, term2Aux;
                    term1Aux = (eqDif[0] * (-1)) / eqDif[1];
                    term2Aux = (eqDif[2] * (-1)) / eqDif[1];

                    a = 1 + (term1Aux* term1Aux);
                    b = eqA[0] + (2 * term1Aux * term2Aux) + (eqA[1] * term1Aux);
                    c = eqA[2] + (term2Aux * term2Aux) + (eqA[1] * term2Aux);

                    pos1.X = ((b * (-1)) + Math.Sqrt((b * b) - (4 * a * c))) / (2 * a);
                    pos2.X = ((b * (-1)) - Math.Sqrt((b * b) - (4 * a * c))) / (2 * a);
                    
                    a = 1;
                    b = eqB[1];
                    c = (pos1.X * pos1.X) + (eqB[0] * pos1.X) + eqB[2];

                    double Y1Aux, Y2Aux, Y3Aux, Y4Aux;

                    Y1Aux = Round(((b * (-1)) + Math.Sqrt((b * b) - (4 * a * c))) / (2 * a));
                    Y2Aux = Round(((b * (-1)) - Math.Sqrt((b * b) - (4 * a * c))) / (2 * a));
                    
                    a = 1;
                    b = eqB[1];
                    c = (pos2.X * pos2.X) + (eqB[0] * pos2.X) + eqB[2];

                    Y3Aux = Round(((b * (-1)) + Math.Sqrt((b * b) - (4 * a * c))) / (2 * a));
                    Y4Aux = Round(((b * (-1)) - Math.Sqrt((b * b) - (4 * a * c))) / (2 * a));

                    //Verifico cuales de todos los Y satisfacen el sistema.
                    term1Aux = Round((pos1.X * pos1.X) + (eqA[0] * pos1.X) + (Y1Aux * Y1Aux) + (eqA[1] * Y1Aux) + eqA[2]);
                    term2Aux = Round((pos1.X * pos1.X) + (eqB[0] * pos1.X) + (Y1Aux * Y1Aux) + (eqB[1] * Y1Aux) + eqB[2]);
                    if (term1Aux == 0 && term2Aux ==0) {
                        pos1.Y = Math.Round(Y1Aux,2);
                    }

                    term1Aux = Round((pos1.X * pos1.X) + (eqA[0] * pos1.X) + (Y2Aux * Y2Aux) + (eqA[1] * Y2Aux) + eqA[2]);
                    term2Aux = Round((pos1.X * pos1.X) + (eqB[0] * pos1.X) + (Y2Aux * Y2Aux) + (eqB[1] * Y2Aux) + eqB[2]);
                    if (term1Aux == 0 && term2Aux == 0){
                        pos1.Y = Math.Round(Y2Aux,2);
                    }

                    term1Aux = Round((pos2.X * pos2.X) + (eqA[0] * pos2.X) + (Y3Aux * Y3Aux) + (eqA[1] * Y3Aux) + eqA[2]);
                    term2Aux = Round((pos2.X * pos2.X) + (eqB[0] * pos2.X) + (Y3Aux * Y3Aux) + (eqB[1] * Y3Aux) + eqB[2]);
                    if (term1Aux == 0 && term2Aux == 0){
                        pos2.Y = Round(Y3Aux);
                    }

                    term1Aux = Round((pos2.X * pos2.X) + (eqA[0] * pos2.X) + (Y4Aux * Y4Aux) + (eqA[1] * Y4Aux) + eqA[2]);
                    term2Aux = Round((pos2.X * pos2.X) + (eqB[0] * pos2.X) + (Y4Aux * Y4Aux) + (eqB[1] * Y4Aux) + eqB[2]);
                    if (term1Aux == 0 && term2Aux == 0){
                        pos2.Y = Round(Y4Aux);
                    }

                    pos1.X = Round(pos1.X);
                    pos2.X = Round(pos2.X);
                }

                return true;
            }
            catch (Exception){
                return false;
            }
        }

        /// <summary>
        /// Función que se encarga de descifrar el mensaje transmitido
        /// </summary>
        /// <param name="sat1">Mensaje captado por el satelite 1</param>
        /// <param name="sat2">Mensaje captado por el satelite 2</param>
        /// <param name="sat3">Mensaje captado por el satelite 3</param>
        /// <returns>devuelve el mensaje descifrado si lo pudo resolver o vacio si no pudo</returns>
        public string GetMessage(string[] sat1, string[] sat2, string[] sat3) {
            string originalMsg = "";

            try
            {
                string[] msg;

                //Quito posibles espacios en blanco al principio y al final de cada palabra
                TrimArray(ref sat1);
                TrimArray(ref sat2);
                TrimArray(ref sat3);

                //Dimensiono un array auxiliar con el menor de los 3 mensajes recibidos
                if (sat1.Length < sat2.Length && sat1.Length < sat3.Length)
                    msg = new string[sat1.Length];
                else if (sat2.Length < sat1.Length && sat2.Length < sat3.Length)
                    msg = new string[sat2.Length];
                else 
                    msg = new string[sat3.Length];
                
                //Verifico que mensajes son los que están desfasados y los corrijo 
                if (sat1[0] == "" && sat1.Length > msg.Length)
                    sat1 = DeleteFirstPos(sat1);

                if (sat2[0] == "" && sat2.Length > msg.Length)
                    sat2 = DeleteFirstPos(sat2);

                if (sat3[0] == "" && sat3.Length > msg.Length)
                    sat3 = DeleteFirstPos(sat3);

                //Una vez correjido todo rearmo el mensaje
                for (int i = 0; i < sat1.Length; i++){
                    if (sat1[i] != "")
                        msg[i] = sat1[i];
                }

                for (int i = 0; i < sat2.Length; i++){
                    if (msg[i] == null && sat2[i] != "")
                        msg[i] = sat2[i];
                }

                for (int i = 0; i < sat3.Length; i++){
                    if (msg[i] == null &&  sat3[i] != "")
                        msg[i] = sat3[i];
                }

                //Paso el vector a string descartando las posiciones vacias
                for (int i = 0; i < msg.Length; i++){
                    if (msg[i] != null)
                        originalMsg = originalMsg + msg[i] + " "; 
                }

                return originalMsg.Trim();

            }
            catch (Exception){
                originalMsg = "";
            }

            return originalMsg;
        }

        /// <summary>
        /// Función que elimina la primer posición de un array de strins
        /// </summary>
        private string[] DeleteFirstPos(string[] arr)
        {
            string[] aux = new string[arr.Length - 1];

            for (int i = 0; i < aux.Length; i++){
                aux[i] = arr[i+1];
            }

            return aux;
        }

        /// <summary>
        /// Función que quita los caracteres de espacio en blanco del principio y del final del contenido de cada posicion de un array de string
        /// </summary>
        private void TrimArray(ref string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arr[i].Trim();
            }
        }

        /// <summary>
        /// Función que redondea un numero cuando le diferencia es menor a 0.001
        /// </summary>
        private double Round(double val)
        {
            double d = Math.Round(val);
            if (Math.Abs(val - d) < 0.001)
                return d;
            else
                return val;
        }

    }
}
