# 🎵 MusicStation

Sistema desktop desenvolvido em **C#**, utilizando **WPF**, **SQL Server** e **Entity Framework Core** para gerenciamento de usuários, empresas e profissionais do ramo musical.

O objetivo do projeto foi aplicar conceitos de desenvolvimento desktop, persistência de dados e organização de código, utilizando uma arquitetura em camadas para facilitar a manutenção e evolução da aplicação.

---

# 🚀 Tecnologias utilizadas

- C#
- .NET
- WPF
- XAML
- SQL Server
- Entity Framework Core
- Fluent API
- Git
- GitHub

---

# ✨ Funcionalidades

- Autenticação de usuários
- Registro de logs da aplicação
- Dashboard administrativo
- Cadastro de usuários
- Cadastro de administradores
- Cadastro de empresas
- Cadastro de profissionais
- Consulta de registros
- Atualização de dados
- Exclusão de registros
- Integração com banco de dados SQL Server
- Controle de permissões por perfil

---

# 🏛 Estrutura do Projeto

O projeto foi organizado em camadas para separar responsabilidades e facilitar a manutenção do código.

```text
MusicStation
│
├── Data
│   └── Contexto do Entity Framework e acesso ao banco
│
├── DTOs
│   └── Objetos de transferência de dados
│
├── Helpers
│   └── Classes auxiliares e utilitárias
│
├── Migrations
│   └── Histórico das migrações do banco de dados
│
├── Models
│   └── Entidades do sistema
│
├── Services
│   └── Regras de negócio e serviços da aplicação
│
├── View
│   └── Interfaces gráficas desenvolvidas em WPF
│
├── ViewModels
│   └── Comunicação entre as Views e a lógica da aplicação (MVVM)
│
├── fotos
│   └── Recursos utilizados pelo sistema
│
├── App.xaml
└── AssemblyInfo.cs
```

---

# 🗄 Banco de Dados

O banco de dados foi desenvolvido utilizando **SQL Server** juntamente com **Entity Framework Core**.

Durante o desenvolvimento foram aplicados conceitos como:

- Relacionamentos entre entidades
- Fluent API
- Migrations
- Constraints
- Índices
- Chaves primárias e estrangeiras

---

# 📚 Conceitos aplicados

Durante o desenvolvimento deste projeto foram utilizados conceitos importantes do ecossistema .NET, como:

- Programação Orientada a Objetos (POO)
- Entity Framework Core
- Fluent API
- Migrations
- SQL Server
- Separação de responsabilidades
- Organização em camadas
- CRUD completo
- Tratamento de exceções
- Boas práticas de organização do código

---

# ▶ Como executar

1. Clone o repositório.

```bash
git clone https://github.com/seu-usuario/MusicStation.git
```

2. Abra a solução no Visual Studio.

3. Configure a Connection String no projeto.

4. Execute as migrations.

```powershell
Update-Database
```

5. Execute a aplicação.

---

# 🔮 Melhorias futuras

- Upload de foto de perfil
- Relatórios em PDF
- Exportação para Excel
- Sistema de notificações
- Tema escuro
- Dashboard com gráficos
- Logs da aplicação
- Testes automatizados

---

# 👨‍💻 Autor

Desenvolvido por Ryan.
