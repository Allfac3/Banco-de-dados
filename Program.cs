// See https://aka.ms/new-console-template for more information

1.Criando o Layout da Planilha no Excel
Primeiro, precisamos criar uma estrutura que permita registrar os esquetes e o progresso da obra. Abaixo está um exemplo de layout básico:

ID do Esquete	Nome da Obra	Data de Início	Data Prevista de Término	Data de Conclusão	Status	Observações
1	Reforma XYZ	01/11/2023	30/11/2023		Em andamento	
2	Construção ABC	05/11/2023	15/12/2023		Não iniciado	
3	Ampliação DEF	15/10/2023	15/11/2023	20/11/2023	Concluído	Atraso de 5 dias
Colunas sugeridas:

ID do Esquete: Identificação única do esquete.
Nome da Obra: Nome do projeto.
Data de Início: Data de início prevista.
Data Prevista de Término: Data planejada para a conclusão.
Data de Conclusão: Data real de término, preenchida apenas quando a obra é concluída.
Status: Status atual(pode ser "Em andamento", "Concluído", "Não iniciado").
Observações: Anotações adicionais, como atraso, motivos de prorrogação, etc.
Salve essa planilha como, por exemplo, esquete_obra.xlsx.

2. Criando o Script em Python
Agora, usaremos Python para ler, analisar e fazer automações na planilha usando a biblioteca Pandas para manipulação de dados e openpyxl para acessar arquivos do Excel. Vamos criar um script que:

Leia o banco de dados de esquetes.
Realize cálculos, como identificar esquetes atrasados.
Gere relatórios automáticos.
Atualize status com base nas datas.
Código em Python
Primeiro, certifique-se de ter as bibliotecas instaladas:

bash
Copiar código
pip install pandas openpyxl
Código do Script
Aqui está um exemplo de script para carregar, processar e atualizar a planilha de esquetes:

python
Copiar código
import pandas as pd
from datetime import datetime

# Carregar a planilha de esquetes
file_path = 'esquete_obra.xlsx'
df = pd.read_excel(file_path)

# Função para atualizar o status dos esquetes
def atualizar_status(row):
    if pd.isnull(row['Data de Conclusão']) and row['Data Prevista de Término'] < datetime.now():
        return 'Atrasado'
    elif pd.isnull(row['Data de Conclusão']):
        return 'Em andamento'
    else:
        return 'Concluído'

# Aplicar a função de atualização de status
df['Status'] = df.apply(atualizar_status, axis = 1)

# Função para calcular dias de atraso
def calcular_atraso(row):
    if pd.notnull(row['Data de Conclusão']) and row['Data de Conclusão'] > row['Data Prevista de Término']:
        return (row['Data de Conclusão'] - row['Data Prevista de Término']).days
    return 0

# Calcular atraso e armazenar em uma nova coluna
df['Dias de Atraso'] = df.apply(calcular_atraso, axis = 1)

# Salvar as atualizações de volta no arquivo Excel
with pd.ExcelWriter(file_path, engine = 'openpyxl', mode = 'a', if_sheet_exists = 'replace') as writer:
    df.to_excel(writer, index = False, sheet_name = 'Esquetes Atualizados')

print("Planilha atualizada com sucesso!")
Explicação do Código
Leitura da Planilha: Carrega a planilha esquete_obra.xlsx usando pandas.
Atualização de Status: Define a função atualizar_status() para ajustar o status do esquete baseado nas datas de término.
Cálculo de Atraso: Define calcular_atraso() para calcular os dias de atraso, caso a obra tenha sido concluída após a data prevista.
Salvar Atualizações: Salva o dataframe atualizado de volta na planilha Excel.
3. Automatizando o Relatório de Atrasos
Você pode expandir o script para gerar relatórios automáticos de obras atrasadas:

python
Copiar código
# Filtrar esquetes atrasados
esquetes_atrasados = df[df['Status'] == 'Atrasado']

# Exportar relatório de esquetes atrasados para Excel
with pd.ExcelWriter('relatorio_atrasos.xlsx', engine = 'openpyxl') as writer:
    esquetes_atrasados.to_excel(writer, index = False, sheet_name = 'Atrasos')

print("Relatório de atrasos gerado com sucesso!")
4.Executando o Script
Salve o script como controle_esquete.py e execute-o em sua máquina com:

bash
Copiar código
python controle_esquete.py
Ao final, você terá:

A planilha esquete_obra.xlsx atualizada com status e dias de atraso.
Um relatório relatorio_atrasos.xlsx com todos os esquetes que estão atrasados.
Essa estrutura oferece controle básico e relatórios rápidos para o acompanhamento de obras.