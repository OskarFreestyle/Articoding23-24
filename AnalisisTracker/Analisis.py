
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.patches as patches
import json

def leer_json(nombre_archivo):
    try:
        with open(nombre_archivo, 'r') as archivo:
            datos = json.load(archivo)
            return datos
    except FileNotFoundError:
        print(f"El archivo '{nombre_archivo}' no se encontró.")
        return None
    except json.JSONDecodeError:
        print(f"El archivo '{nombre_archivo}' no es un JSON válido.")
        return None


def main():
    # Lectura del archivo json
    # nombre_archivo = "tracesPrueba4ESO.json"  # Nombre del archivo JSON
    nombre_archivo = "col2.json"  # Nombre del archivo JSON

    datos = leer_json(nombre_archivo)

    # El tamaño de las trazas
    tam = len(datos)
    print(f"El número de trazas es: {tam}")

    # Prueba con 10 elementos
    lista_nombres = []
    lista_primer_timestamp = []
    lista_ultimo_timestamp = []
    lista_ultimo_nivel = []
    lista_ultimo_nivel_index = []
    index = 0

    for traza in datos:
        index += 1
        print(index)

        # Se determina el name de la traza
        if "actor" in traza:
            actor = traza["actor"]
            if "name" in actor:
                name = actor["name"]

                # Si el nombre no está en la lista se añade y también se añade su primer timestamp
                if name not in lista_nombres:
                    lista_nombres.append(name)
                    lista_primer_timestamp.append(traza["timestamp"][11:16])
                    lista_ultimo_timestamp.append(traza["timestamp"][11:16])
                    lista_ultimo_nivel.append("")
                    lista_ultimo_nivel_index.append(0)
                # Si el nombre ya se encuentra en la lista, se actualiza su ultimo timestamp
                else:
                    name_index = lista_nombres.index(name)
                    lista_ultimo_timestamp[name_index] = traza["timestamp"][11:16]
                
                # Si se trata de un nivel completado se anota en la lista de niveles
                if "verb" in traza:
                    verb = traza["verb"]
                    if "id" in verb:
                        id = verb["id"]
                        
                        # El jugador ha intentado pasarse un nivel
                        if id.split("/")[-1] == "completed":
                            if "result" in traza:
                                result = traza["result"]
                                if "success" in result:
                                    success = result["success"]

                                    # Si el resultado true se trata de un nivel completado
                                    if success == True:
                                        print("successs")
                                        name_index = lista_nombres.index(name)

                                        # Tambien se comprueba que se trate de un nivel
                                        if traza["object"]["definition"]["type"].split("/")[-1] == "level":
                                            lista_ultimo_nivel[name_index] = traza["object"]["id"].split("/")[-1] 
                                            lista_ultimo_nivel_index[name_index] = lista_ultimo_nivel_index[name_index] + 1




    # Se descarta el primer elemento porque no era un alumno
    lista_nombres = lista_nombres[1:]
    lista_primer_timestamp = lista_primer_timestamp[1:]
    lista_ultimo_timestamp = lista_ultimo_timestamp[1:]
    lista_ultimo_nivel = lista_ultimo_nivel[1:]
    lista_ultimo_nivel_index = lista_ultimo_nivel_index[1:]

    # Muestra de las listas recogidas
    print(lista_nombres)
    print(lista_primer_timestamp)
    print(lista_ultimo_timestamp)
    print(lista_ultimo_nivel)
    print(lista_ultimo_nivel_index)

    lista_tiempos = []
    index = 0
    for i in lista_nombres:
        
        tiempo_primer_timestamp = int(lista_primer_timestamp[index][0:2]) * 60 + int(lista_primer_timestamp[index][3:5])
        tiempo_ultimo_timestamp = int(lista_ultimo_timestamp[index][0:2]) * 60 + int(lista_ultimo_timestamp[index][3:5])

        lista_tiempos.append(tiempo_ultimo_timestamp - tiempo_primer_timestamp)
        index += 1

    print(lista_tiempos)

    # Establecer el rango para el eje x (por ejemplo, de 0 a 6) y el eje y (por ejemplo, de 5 a 15)
    plt.xlim(0, 50)
    plt.ylim(0, 100)
    plt.xlim(5, 25)
    plt.ylim(40, 75)

    # plt.xlim(0, 100)
    # plt.ylim(0, 100)

    # Etiquetas de los ejes
    plt.xlabel("Nivel superado (unidad)")
    plt.ylabel("Tiempo de juego (minutos)")

    # Título del gráfico
    plt.title("Tiempo jugado vs Nivel superado")

    # Etiquetas de nombres
    # for i, nombre in enumerate(lista_nombres):
    #     plt.text(lista_ultimo_nivel_index[i], lista_tiempos[i], nombre, fontsize=8, ha='right')
    
    # Crear y añadir la franja diagonal al gráfico
    # coordenadas1 = [(0, 0), (15, 0), (32.5, 160), (0, 160)]
    # coordenadas2 = [(15, 0), (32.5, 0), (50, 160), (32.5, 160)]
    # coordenadas3 = [(32.5, 0), (100, 0), (100, 160), (50, 160)]

    coordenadas1 = [(0, 0), (0, 0), (28, 100), (0, 100)]
    coordenadas2 = [(0, 0), (0, 0), (49, 100), (28, 100)]
    coordenadas3 = [(0, 0), (0, 0), (100, 100), (49, 100)]

    diagonal_patch1 = patches.Polygon(coordenadas1, closed=True, color='red', alpha=0.5)
    diagonal_patch2 = patches.Polygon(coordenadas2, closed=True, color='yellow', alpha=0.5)
    diagonal_patch3 = patches.Polygon(coordenadas3, closed=True, color='green', alpha=0.5)

    plt.gca().add_patch(diagonal_patch1)
    plt.gca().add_patch(diagonal_patch2)
    plt.gca().add_patch(diagonal_patch3)

    # # Gráfico con 2 ejes, tiempo jugado y nivel alcanzado
    plt.scatter(lista_ultimo_nivel_index, lista_tiempos)
    
    # # MAPEO DE COLORES
    # lista_gustos = [ "Sí", "Sí", "Sí", "Sí", "No", "No", "Sí", "Sí", "No", "No", "Sí", "Sí", "Sí", "No","No"]
    # colores = {'Sí': 'purple', 'No': 'blue'}  # Diccionario para mapear los colores según los valores

    # # Iteramos sobre la lista de gustos y creamos el scatter plot con colores según el valor
    # for i, gusto in enumerate(lista_gustos):
    #     plt.scatter(lista_ultimo_nivel_index[i], lista_tiempos[i], color=colores[gusto])


    # Mostrar el gráfico
    plt.show()


main()

