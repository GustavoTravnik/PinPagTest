```markdown
# PinPagTest

## 🛠️ Tecnologias Utilizadas
- .NET Core 8.0  
- PostgreSQL  
- Entity Framework Core  
- xUnit  
- Docker  
- pgAdmin  
- EF Migrations  
- OpenAPI  

---

## 🚀 Como Executar o Projeto

### **Usando Docker**
1. Navegue até o diretório `\BankServices`.  
2. Execute o comando:
   bash
   docker-compose up
   Isso irá iniciar a aplicação juntamente com o banco de dados PostgreSQL e a interface pgAdmin para gerenciamento do banco.

3. **Migrations:**  
   As migrations devem ser aplicadas automaticamente ao iniciar o projeto.  
   - Caso isso não aconteça, você pode criar o banco manualmente executando o script localizado em:

     \Database\CreateDatabaseScript.sql
```

### **Executando Localmente**
1. Abra o arquivo `.sln` localizado no diretório `\BankServices`.  
2. Esta solução já inclui:
   - Projeto principal  
   - Projeto de testes  
   - Migrations do EF  
   - Configuração do Docker Compose  

3. Execute o projeto diretamente pelo Visual Studio ou pela IDE de sua preferência.

---

## 🧪 Executando Testes
Os testes unitários e de integração estão disponíveis no projeto `BankServiceTests`.

---

## 📚 Uso da API
Como a API está totalmente documentada no Swagger, não há necessidade de listar todas as URLs aqui. Abaixo estão detalhes importantes sobre seu funcionamento:

### Campos e Funcionamento
- **Document:** Refere-se ao CPF. Pode ser enviado com ou sem máscara, sendo corretamente validado pelo sistema.

- **Objeto Account:**  
  Utilizado para operações com usuários, contendo dois campos:
  - `Id`: Identificador no banco de dados  
  - `Document`: CPF  
  **Observação:** Apenas um desses campos deve ser preenchido para evitar ambiguidades.

- **Tipo de Transação:**  
  Representado por um Enum:
  - `0`: Depósito  
  - `1`: Retirada  

- **Formato de Data:**  
  Segue o padrão ISO 8601, por exemplo:
  - `yyyy-MM-dd`  
  - `yyyy-MM-ddTHH:mm:ss`

- **Campo Identifyer:**  
  Usado nas URLs das requisições (por exemplo, `/api/ClientAccount/getTransactionList/{identifyer}`).  
  Esse campo pode receber:
  - O `Id` do banco  
  - O número do CPF  

---

Aproveite o projeto PinPagTest! 🎉
