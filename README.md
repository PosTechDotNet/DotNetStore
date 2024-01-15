# Processamento de Pedido com Durable Function

## Índice
- [Processamento de Pedido com Durable Function](#processamento-de-pedido-com-durable-function)
  - [Índice](#índice)
  - [Sobre](#sobre)
  - [Integrantes](#integrantes)
  - [Solução](#solução)
    - [Fluxograma da Solução](#fluxograma-da-solução)
  - [Como Executar o Projeto](#como-executar-o-projeto)
    - [Criação do Azure Table Storage](#criação-do-azure-table-storage)
      - [Plataforma](#plataforma)
      - [Pelo Azure CLI](#pelo-azure-cli)
    - [Execução Local](#execução-local)
      - [Otendo a string de conexão do Table Storage](#otendo-a-string-de-conexão-do-table-storage)
      - [Emulador de Storage ou Plataforma Azure](#emulador-de-storage-ou-plataforma-azure)
      - [Pelo VSCode](#pelo-vscode)
      - [Pelo Visual Studio](#pelo-visual-studio)
      - [Chamada da Função](#chamada-da-função)
      - [Exemplo de um Pedido para testar a aplicação](#exemplo-de-um-pedido-para-testar-a-aplicação)
            
## Sobre
Este projeto faz parte de uma das entregas do trabalho de conclusão da segunda fase da POSTECH FIAP de Arquitetura de Sistemas .Net com Azure.

[voltar](#índice)

## Integrantes
| Nome                    | RM     | GitHub                             |
| ----------------------- | ------ | ---------------------------------- |
| Alex Jussiani Junior    | 350671 | https://github.com/AlexJussiani    |
| Erick Setti dos Santos  | 351206 | https://github.com/ESettiCalculist |
| Fábio da Silva Pereira  | 351053 | https://github.com/fbiopereira     |
| Marcel da Silva Fonseca | 348885 | ==> Trocar pelo Lucas              |
| Richard Kendy Tanaka    | 351234 | https://github.com/RichardKT88     |

[voltar](#índice)

## Solução
Desenvolvimento de uma Durable Function com o Function Chaining Pattern para realizar o processo de fechamento de um pedido. Saiba mais sobre os patterns das [Durable Functions](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=in-process%2Cnodejs-v3%2Cv1-model&pivots=csharp)

[voltar](#índice)

### Fluxograma da Solução
![Fluxograma](DotNetStore/Assets/imgs/Fluxograma.jpg)

[voltar](#índice)

## Como Executar o Projeto
### Criação do Azure Table Storage
#### Plataforma
Para criar uma tabela no Azure Table Storage por meio da plataforma Azure, você pode seguir as etapas abaixo usando o Portal do Azure:

1. Faça login no [Portal do Azure](https://portal.azure.com).

2. No painel de navegação à esquerda, selecione "Azure Storage".

![CriarTableStorage1](DotNetStore/Assets/imgs/Azure_Store_Account1.jpg)

![CriarTableStorage2](DotNetStore/Assets/imgs/Azure_Store_Account2.jpg)

3. Selecione a conta de armazenamento na qual você deseja criar a tabela.

![CriarTableStorage3](DotNetStore/Assets/imgs/Azure_Store_Account3.jpg)

4. Na seção "Data Storage", clique em "Tables" (Tabelas).

![CriarTableStorage4](DotNetStore/Assets/imgs/Azure_Store_Account4.jpg)

5. Clique em "+ Add Table" (Adicionar Tabela).

![CriarTableStorage1](DotNetStore/Assets/imgs/Azure_Store_Account5.jpg)

6. Insira o nome desejado para a tabela - no caso **Pedidos** -  e outras configurações necessárias.

7. Clique em "+ Add Table" (Adicionar Tabela).

#### Pelo Azure CLI
1. Certifique-se de ter feito login na sua conta Azure antes de executar o comando acima. Você pode fazer login usando:
    ```
    az login
    ```
2. Antes é necessário um grupo de recurso (resource group)
   ```
    az group create --name <NOME_DA_RECURSO> --location <LOCALIZACAO>
   ```
   ![ResourceGroup](DotNetStore/Assets/imgs/Resource_Group.jpg)
3. Em seguida devemos criar uma conta de armazenamento (storage account) usando a Azure CLI, você pode usar o seguinte comando:
   ```
    az storage account create --name <NOME_DA_CONTA> --resource-group <NOME_GRUPO_DE_RECURSOS> --location <LOCALIZACAO> --sku <TIPO_DE_SKU>
   ```
   ![StorageAccount](DotNetStore/Assets/imgs/Storage_Account.jpg)
4. Para criar uma tabela no Azure Table Storage via linha de comando, você pode usar o Azure CLI. Abaixo está um exemplo do comando para criar uma tabela:
    ```
      az storage table create --account-name <NOME_DA_CONTA> --name <NOME_DA_TABELA>
    ```
    ![StorageTable](DotNetStore/Assets/imgs/Storage_Table.jpg)

5. Lembre-se de substituir **NOME_DA_CONTA** pelo nome da sua conta de armazenamento e **NOME_DA_TABELA** pelo nome desejado para a tabela. No caso o nome será **Pedidos**.

6. Se você ainda não instalou o Azure CLI, pode fazer o download e instalá-lo a partir do site oficial: [Azure CLI](https://learn.microsoft.com/pt-br/cli/azure/install-azure-cli).

[voltar](#índice)

### Execução Local

#### Otendo a string de conexão do Table Storage
1. Na aba à esquerda no campo Security + Networking clique em Access Keys
   
   ![stringConexao1](DotNetStore/Assets/imgs/Azure_Store_Account6.jpg)

2. Clique no botão Show (Mostrar) na Connection String e copie a string de conexão
   
   ![stringConexao2](DotNetStore/Assets/imgs/Azure_Store_Account7.jpg)

3. Pelo Azure CLI para obter a connection string utilize o seguinte comando.
   ```
    az storage account show-connection-string --name NOME_DA_CONTA --resource-group NOME_GRUPO_DE_RECURSOS
   ```

#### Emulador de Storage ou Plataforma Azure
1. Primeiro devemos criar um arquivo json com o seguinte nome **local.settings.json** e inserir os dados abaixo.
   ```
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true", ==> Pode ser colocada a string de conexão da Storage Account feito na plataforma ou pelo Azure CLI
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "AzTbStorageConnectionString": "" ==> A string de conexão do Storage Account feito na plataforma ou pelo Azure CLI
      }
    }
   ```
2. O **local.settings.json** pode conter informações sensíveis, como cadeias de conexão e segredos. Por essa razão, é altamente recomendável que você nunca armazene esse arquivo em um repositório remoto, a fim de evitar a exposição de dados confidenciais.
3. Para executar o projeto no seu ambiente local, você pode usar o Emulador Azurite para a configuração de conexão AzureWebJobsStorage, exigida pelo projeto.
4. Para utilizar o emulador, defina o valor de AzureWebJobsStorage como **UseDevelopmentStorage=true**. Altere essa configuração para uma cadeia de conexão de conta de armazenamento antes da implantação.
![LocalSettings](DotNetStore/Assets/imgs/local_settings.jpg)

***Para obter mais informações, confira [“Emulador de armazenamento local”](https://learn.microsoft.com/pt-br/azure/storage/common/storage-use-azurite?tabs=visual-studio-code%2Ctable-storage).***

#### Pelo VSCode
1. Para executar uma função do Azure Functions no Visual Studio Code, você precisará instalar a extensão do Azure Functions no VSCode.
   1. Abra o VSCode.
   2. No menu lateral, clique no ícone de extensões (ícone de quebra-cabeça) ou pressione Ctrl (Command) + Shift + X.
   3. Procure por "Azure Functions" e instale a extensão fornecida pela Microsoft.
2. Abra o terminal no VSCode pressionando os comandos Ctrl (Command) + Shift + `
3. Navegue até a pasta do seu projeto do Azure Functions.
4. Execute o seguinte comando:
```
func start

```

#### Pelo Visual Studio
1.
2.
3.
4.
5.

#### Chamada da Função
Com a função em execução, você pode chamar suas funções HTTP ou ativar eventos para outras funções. Observe que, para funções HTTP, você precisará de uma ferramenta como o Postman para testar chamadas HTTP localmente.

![Postman](DotNetStore/Assets/imgs/Execucao_Local_2.jpg)

#### Exemplo de um Pedido para testar a aplicação

```
{
    "UsuarioId": 1,
    "Produtos": [
        {
            "SKU": 1,
            "Quantidade": 1,
            "ValorUnitario": 200
        },
        {
            "SKU": 5,
            "Quantidade": 1,
            "ValorUnitario": 300
        },
        {
            "SKU": 55,
            "Quantidade": 2,
            "ValorUnitario": 1
        }
    ],
    "Endereco": {
        "cep": "12345-000",
        "numero": 434,
        "complemento": "qualquer coisa"
    }
}
```

[voltar](#índice)


