USE MusicStation
SELECT * FROM Usuarios
SELECT * FROM Administradores
SELECT * FROM Clientes
SELECT * FROM Profissionais
SELECT * FROM Empresas
SELECT * FROM Pagamentos


-- Definindo o tipo de log "Login"
INSERT INTO TiposAcao(Nome)
VALUES ('Login')

-- INSERT DE ALGUNS USUÁRIOS
INSERT INTO Usuarios(Nome,SenhaHash,NomeUsuario,Email)
VALUES ('Ryan','111','ryan','ryan@gmail.com')

INSERT INTO Usuarios(Nome,SenhaHash,NomeUsuario,Email)
VALUES ('Anahi Alexandra','111','mei','anahi@gmail.com')

INSERT INTO Usuarios(Nome,SenhaHash,NomeUsuario,Email)
VALUES ('Larrisa','111','lari','larissa@gmail.com')

INSERT INTO Administradores(Id,NivelAcesso,Observacao)
VALUES (1,'AdministradorGeral', 'Adm com maior acesso')

INSERT INTO Administradores(Id,NivelAcesso,Observacao)
VALUES (2,'AdministradorGeral', 'Adm com maior acesso')

INSERT INTO Administradores(Id,NivelAcesso,Observacao)
VALUES (3,'Financeiro', 'Finanças')

-- INSERT Clientes, Profissionais e Empresas

 
 -- =========================================
-- USUARIOS
-- IDs começarão do 4
-- Ativo e DataCadastro são automáticos
-- =========================================

INSERT INTO Usuarios
(Nome, Email, NomeUsuario, SenhaHash)
VALUES
('João Silva', 'joao.silva@email.com', 'joaosilva', '123456'),
('Maria Oliveira', 'maria.oliveira@email.com', 'mariaoliveira', '123456'),
('Carlos Souza', 'carlos.souza@email.com', 'carlossouza', '123456'),
('Ana Lima', 'ana.lima@email.com', 'analima', '123456'),
('Lucas Pereira', 'lucas.p@email.com', 'lucasp', '123456'),
('Fernanda Rocha', 'fernanda.r@email.com', 'fernandar', '123456'),
('Ricardo Alves', 'ricardo.a@email.com', 'ricardoalves', '123456'),
('Patricia Gomes', 'patricia.g@email.com', 'patriciag', '123456'),
('Bruno Martins', 'bruno.m@email.com', 'brunom', '123456'),
('Juliana Costa', 'juliana.c@email.com', 'julianac', '123456'),

('Pedro Henrique', 'pedro.h@email.com', 'pedroh', '123456'),
('Camila Santos', 'camila.s@email.com', 'camilas', '123456'),
('Rafael Mendes', 'rafael.m@email.com', 'rafaelm', '123456'),
('Larissa Prado', 'larissa.p@email.com', 'larissap', '123456'),
('Diego Costa', 'diego.c@email.com', 'diegoc', '123456'),
('Beatriz Lima', 'beatriz.l@email.com', 'beatrizl', '123456'),
('Thiago Alves', 'thiago.a@email.com', 'thiagoa', '123456'),
('Amanda Ferreira', 'amanda.f@email.com', 'amandaf', '123456'),
('Felipe Rocha', 'felipe.r@email.com', 'feliper', '123456'),
('Gabriela Martins', 'gabriela.m@email.com', 'gabrielam', '123456'),

('Sound Tech LTDA', 'contato@soundtech.com', 'soundtech', '123456'),
('Music House LTDA', 'contato@musichouse.com', 'musichouse', '123456'),
('Studio Beats LTDA', 'contato@studiobeats.com', 'studiobeats', '123456'),
('Power Music LTDA', 'contato@powermusic.com', 'powermusic', '123456'),
('Live Eventos LTDA', 'contato@liveeventos.com', 'liveeventos', '123456'),
('Audio Prime LTDA', 'contato@audioprime.com', 'audioprime', '123456'),
('Mix Produções LTDA', 'contato@mixproducoes.com', 'mixproducoes', '123456'),
('Music Center LTDA', 'contato@musiccenter.com', 'musiccenter', '123456'),
('Top Studio LTDA', 'contato@topstudio.com', 'topstudio', '123456'),
('Master Audio LTDA', 'contato@masteraudio.com', 'masteraudio', '123456');


-- =========================================
-- CLIENTES
-- IDs 4 até 13
-- =========================================

INSERT INTO Clientes
(Id, Rua, Numero, Cidade)
VALUES
(4,  'Rua das Flores', '120', 'São Paulo'),
(5,  'Rua Azul', '45', 'Osasco'),
(6,	 'Av Central', '300', 'Barueri'),
(7,  'Rua Verde', '88', 'Carapicuíba'),
(8,  'Rua Horizonte', '12', 'Cotia'),
(9,  'Rua Aurora', '77', 'São Paulo'),
(10, 'Rua das Palmeiras', '220', 'Taboão'),
(11, 'Rua da Paz', '65', 'Suzano'),
(12, 'Rua do Lago', '91', 'Mauá'),
(13, 'Rua Bela Vista', '55', 'Diadema');


-- =========================================
-- PROFISSIONAIS
-- IDs 14 até 23
-- =========================================

INSERT INTO Profissionais
(Id, Descricao, Especialidade, Endereco, EmpresaId)
VALUES
(14, 'Guitarrista profissional', 'Guitarra', 'Rua A, 10 - São Paulo', 24),

(15, 'Professora de piano', 'Piano',
 'Rua B, 22 - Osasco', NULL),

(16, 'Baterista experiente', 'Bateria',
 'Rua C, 33 - Barueri', 25),

(17, 'Cantora e compositora', 'Canto',
 'Rua D, 44 - Cotia', NULL),

(18, 'Técnico de áudio', 'Áudio',
 'Rua E, 55 - Suzano', 26),

(19, 'Violinista clássica', 'Violino',
 'Rua F, 66 - Mauá', NULL),

(20, 'Produtor musical', 'Produção Musical',
 'Rua G, 77 - Diadema', 27),

(21, 'Instrutora vocal', 'Técnica Vocal',
 'Rua H, 88 - Taboão', NULL),

(22, 'Baixista profissional', 'Baixo',
 'Rua I, 99 - Carapicuíba', 28),

(23, 'DJ e produtora', 'DJ / Produção Musical',
 'Rua J, 100 - São Paulo', NULL);


INSERT INTO Empresas
(Id, Cnpj, NomeFantasia, Endereco)
VALUES
(24, '12.345.678/0001-01', 'Sound Tech', 'Av Paulista, 100'),
(25, '12.345.678/0001-02', 'Music House','Rua Augusta, 200'),
(26, '12.345.678/0001-03', 'Studio Beats', 'Av Brasil, 300'),
(27, '12.345.678/0001-04', 'Power Music',  'Rua das Nações, 400'),
(28, '12.345.678/0001-05', 'Live Eventos',  'Av Europa, 500'),
(29, '12.345.678/0001-06', 'Audio Prime', 'Rua Central, 600'),
(30, '12.345.678/0001-07', 'Mix Produções', 'Rua do Comércio, 700'),
(31, '12.345.678/0001-08', 'Music Center', 'Av Independência, 800'),
(32, '12.345.678/0001-09', 'Top Studio', 'Rua XV, 900'),
(33, '12.345.678/0001-10', 'Master Audio', 'Av Atlântica, 1000');



INSERT INTO Pagamentos
(
    Valor,
    DataVencimento,
    DataPagamento,
    FormaPagamentoId,
    StatusPagamentoId,
    CategoriaPagamentoId,
    Observacoes,
    ClienteId,
    EmpresaId,
    ProfissionalId
)
VALUES
-- JANEIRO
(180.00, '2026-01-05', '2026-01-05', 1, 2, 1, 'Aula de violão iniciante com profissional', 4, NULL, 14),
(320.00, '2026-01-12', '2026-01-12', 2, 2, 2, 'Locação de guitarra para evento', 5, 24, NULL),
(750.00, '2026-01-22', '2026-01-22', 1, 2, 3, 'Venda de violão acústico', 6, 24, NULL),

-- FEVEREIRO
(250.00, '2026-02-03', '2026-02-03', 1, 2, 1, 'Regulagem de guitarra com profissional', 7, NULL, 15),
(480.00, '2026-02-10', '2026-02-10', 3, 2, 2, 'Locação de teclado Yamaha', 8, 25, NULL),
(1200.00, '2026-02-18', '2026-02-18', 2, 2, 3, 'Venda de interface de áudio', 9, 25, NULL),
(90.00, '2026-02-26', NULL, 5, 1, 1, 'Pagamento pendente de aula avulsa', 10, NULL, 16),

-- MARÇO
(300.00, '2026-03-04', '2026-03-04', 1, 2, 1, 'Aula de guitarra avançada', 11, NULL, 17),
(650.00, '2026-03-11', '2026-03-11', 2, 2, 2, 'Locação de bateria Pearl', 12, 26, NULL),
(1600.00, '2026-03-19', '2026-03-19', 1, 2, 3, 'Venda de teclado usado', 13, 26, NULL),
(120.00, '2026-03-25', NULL, 4, 3, 4, 'Multa por atraso na devolução', 4, 24, NULL),

-- ABRIL
(420.00, '2026-04-02', '2026-04-02', 1, 2, 1, 'Gravação em estúdio com técnico de áudio', 5, NULL, 18),
(700.00, '2026-04-09', '2026-04-09', 3, 2, 2, 'Locação de kit de microfones', 6, 27, NULL),
(980.00, '2026-04-17', '2026-04-17', 2, 2, 3, 'Venda de pedal de efeito', 7, 27, NULL),
(210.00, '2026-04-28', NULL, 5, 1, 2, 'Locação pendente de caixa amplificada', 8, 28, NULL),

-- MAIO
(500.00, '2026-05-06', '2026-05-06', 1, 2, 1, 'Serviço de mixagem', 9, NULL, 20),
(900.00, '2026-05-13', '2026-05-13', 2, 2, 2, 'Locação de equipamento para show', 10, 29, NULL),
(2100.00, '2026-05-20', '2026-05-20', 1, 2, 3, 'Venda de guitarra semi-nova', 11, 30, NULL),
(150.00, '2026-05-29', NULL, 4, 3, 4, 'Multa por atraso de instrumento', 12, 29, NULL),

-- JUNHO
(650.00, '2026-06-03', '2026-06-03', 1, 2, 1, 'Produção musical básica', 13, NULL, 21),
(1100.00, '2026-06-08', '2026-06-08', 3, 2, 2, 'Locação de bateria para gravação', 4, 31, NULL),
(2700.00, '2026-06-14', '2026-06-14', 2, 2, 3, 'Venda de baixo elétrico', 5, 32, NULL),
(350.00, '2026-06-17', '2026-06-17', 1, 2, 1, 'Aula intensiva de violão', 6, NULL, 22),
(280.00, '2026-06-21', NULL, 5, 1, 2, 'Locação pendente de amplificador', 7, 33, NULL);


USE MusicStation;
GO

BEGIN TRANSACTION;

BEGIN TRY

    ------------------------------------------------------------
    -- SERVIÇOS
    ------------------------------------------------------------

    INSERT INTO Servicos
    (
        EmpresaId,
        Nome,
        Descricao,
        Preco,
        Ativo
    )
    VALUES
    (24, 'Aula de Guitarra',
        'Aula individual de guitarra para alunos iniciantes e intermediários.',
        180.00, 1),

    (25, 'Aula de Piano',
        'Aula de piano com conteúdo teórico e prático.',
        220.00, 1),

    (26, 'Gravação em Estúdio',
        'Gravação profissional de voz e instrumentos.',
        500.00, 1),

    (27, 'Produção Musical',
        'Produção completa de música, incluindo arranjo e acompanhamento.',
        850.00, 1),

    (28, 'Sonorização de Evento',
        'Montagem e operação de equipamentos de áudio para eventos.',
        1200.00, 1),

    (29, 'Mixagem',
        'Mixagem profissional de faixas musicais.',
        500.00, 1),

    (30, 'Masterização',
        'Finalização e masterização para plataformas digitais.',
        350.00, 1),

    (31, 'Manutenção de Instrumentos',
        'Revisão, limpeza e pequenos reparos em instrumentos musicais.',
        280.00, 1),

    (32, 'Consultoria de Áudio',
        'Consultoria para montagem e configuração de equipamentos de áudio.',
        400.00, 1),

    (33, 'Gravação de Podcast',
        'Captação, edição e tratamento de áudio para podcast.',
        450.00, 0);


    ------------------------------------------------------------
    -- RECUPERANDO OS IDS DOS SERVIÇOS
    ------------------------------------------------------------

    DECLARE @AulaGuitarraId INT =
    (
        SELECT IdServico
        FROM Servicos
        WHERE EmpresaId = 24
          AND Nome = 'Aula de Guitarra'
    );

    DECLARE @AulaPianoId INT =
    (
        SELECT IdServico
        FROM Servicos
        WHERE EmpresaId = 25
          AND Nome = 'Aula de Piano'
    );

    DECLARE @GravacaoEstudioId INT =
    (
        SELECT IdServico
        FROM Servicos
        WHERE EmpresaId = 26
          AND Nome = 'Gravação em Estúdio'
    );

    DECLARE @ProducaoMusicalId INT =
    (
        SELECT IdServico
        FROM Servicos
        WHERE EmpresaId = 27
          AND Nome = 'Produção Musical'
    );

    DECLARE @SonorizacaoId INT =
    (
        SELECT IdServico
        FROM Servicos
        WHERE EmpresaId = 28
          AND Nome = 'Sonorização de Evento'
    );

    DECLARE @MixagemId INT =
    (
        SELECT IdServico
        FROM Servicos
        WHERE EmpresaId = 29
          AND Nome = 'Mixagem'
    );

    DECLARE @MasterizacaoId INT =
    (
        SELECT IdServico
        FROM Servicos
        WHERE EmpresaId = 30
          AND Nome = 'Masterização'
    );

    DECLARE @ManutencaoId INT =
    (
        SELECT IdServico
        FROM Servicos
        WHERE EmpresaId = 31
          AND Nome = 'Manutenção de Instrumentos'
    );


    ------------------------------------------------------------
    -- PEDIDOS
    ------------------------------------------------------------

    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (4, '2026-01-10', 180.00, 'Concluido');

    DECLARE @Pedido1Id INT = SCOPE_IDENTITY();


    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (5, '2026-02-12', 220.00, 'Concluido');

    DECLARE @Pedido2Id INT = SCOPE_IDENTITY();


    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (6, '2026-03-08', 500.00, 'Concluido');

    DECLARE @Pedido3Id INT = SCOPE_IDENTITY();


    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (7, '2026-04-18', 850.00, 'EmAndamento');

    DECLARE @Pedido4Id INT = SCOPE_IDENTITY();


    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (8, '2026-05-09', 1200.00, 'EmAndamento');

    DECLARE @Pedido5Id INT = SCOPE_IDENTITY();


    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (9, '2026-06-11', 850.00, 'Pendente');

    DECLARE @Pedido6Id INT = SCOPE_IDENTITY();


    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (10, '2026-06-22', 280.00, 'Cancelado');

    DECLARE @Pedido7Id INT = SCOPE_IDENTITY();


    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (11, '2026-07-05', 500.00, 'Pendente');

    DECLARE @Pedido8Id INT = SCOPE_IDENTITY();


    ------------------------------------------------------------
    -- ITENS DOS PEDIDOS
    ------------------------------------------------------------

    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido1Id,
        @AulaGuitarraId,
        14,
        180.00,
        'Primeira aula de guitarra do cliente.',
        'Concluido'
    );

    DECLARE @Item1Id INT = SCOPE_IDENTITY();


    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido2Id,
        @AulaPianoId,
        15,
        220.00,
        'Aula introdutória de piano.',
        'Concluido'
    );

    DECLARE @Item2Id INT = SCOPE_IDENTITY();


    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido3Id,
        @GravacaoEstudioId,
        18,
        500.00,
        'Gravação de voz e violão.',
        'Concluido'
    );

    DECLARE @Item3Id INT = SCOPE_IDENTITY();


    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido4Id,
        @ProducaoMusicalId,
        20,
        850.00,
        'Produção musical em fase de arranjo.',
        'EmAndamento'
    );

    DECLARE @Item4Id INT = SCOPE_IDENTITY();


    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido5Id,
        @SonorizacaoId,
        22,
        1200.00,
        'Preparação de sonorização para evento.',
        'EmAndamento'
    );

    DECLARE @Item5Id INT = SCOPE_IDENTITY();


    -- Pedido com dois serviços
    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido6Id,
        @MixagemId,
        20,
        500.00,
        'Mixagem de voz e instrumental.',
        'Pendente'
    );

    DECLARE @Item6Id INT = SCOPE_IDENTITY();


    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido6Id,
        @MasterizacaoId,
        23,
        350.00,
        'Masterização após a conclusão da mixagem.',
        'Pendente'
    );

    DECLARE @Item7Id INT = SCOPE_IDENTITY();


    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido7Id,
        @ManutencaoId,
        22,
        280.00,
        'Pedido cancelado pelo cliente.',
        'Cancelado'
    );

    DECLARE @Item8Id INT = SCOPE_IDENTITY();


    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @Pedido8Id,
        @GravacaoEstudioId,
        18,
        500.00,
        'Aguardando confirmação de horário.',
        'Pendente'
    );

    DECLARE @Item9Id INT = SCOPE_IDENTITY();


    ------------------------------------------------------------
    -- AVALIAÇÕES
    -- Somente serviços concluídos
    ------------------------------------------------------------

    INSERT INTO Avaliacoes
    (
        ServicoPedidoId,
        ClienteId,
        Nota,
        Comentario,
        DataAvaliacao
    )
    VALUES
    (
        @Item1Id,
        4,
        5,
        'Professor muito atencioso e aula excelente.',
        '2026-01-11'
    );


    INSERT INTO Avaliacoes
    (
        ServicoPedidoId,
        ClienteId,
        Nota,
        Comentario,
        DataAvaliacao
    )
    VALUES
    (
        @Item2Id,
        5,
        4,
        'Gostei bastante da aula e da metodologia.',
        '2026-02-13'
    );


    INSERT INTO Avaliacoes
    (
        ServicoPedidoId,
        ClienteId,
        Nota,
        Comentario,
        DataAvaliacao
    )
    VALUES
    (
        @Item3Id,
        6,
        5,
        'Gravação com ótima qualidade e atendimento profissional.',
        '2026-03-10'
    );


    COMMIT TRANSACTION;

    PRINT 'Dados da Gestão de Serviços inseridos com sucesso.';

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT 'Erro ao inserir os dados da Gestão de Serviços.';
    PRINT ERROR_MESSAGE();

END CATCH;
GO


INSERT INTO Servicos
(EmpresaId,ProfissionalId,Nome,Descricao,Preco,Ativo)
VALUES

(24,NULL,
'Locação de Estúdio',
'Estúdio completo para gravação.',
600,1),

(NULL,15,
'Aula de Piano',
'Aulas presenciais de piano.',
220,1),

(25,NULL,
'Mixagem',
'Mixagem profissional.',
480,1),

(NULL,17,
'Aula de Canto',
'Técnica vocal.',
160,1),

(26,NULL,
'Masterização',
'Masterização profissional.',
350,1),

(NULL,20,
'Produção Musical',
'Produção completa.',
850,1);


USE MusicStation;
GO

BEGIN TRANSACTION;

BEGIN TRY

    DECLARE @ServicoId INT;

    SELECT @ServicoId = IdServico
    FROM Servicos
    WHERE ProfissionalId = 15
      AND Nome = 'Aula de Piano';

    IF @ServicoId IS NULL
    BEGIN
        THROW 50001, 'O serviço autônomo não foi encontrado.', 1;
    END;

    --------------------------------------------------
    -- CRIA O PEDIDO
    --------------------------------------------------

    INSERT INTO Pedidos
    (
        ClienteId,
        DataPedido,
        Total,
        Status
    )
    VALUES
    (
        4,
        GETDATE(),
        220.00,
        'Pendente'
    );

    DECLARE @PedidoId INT = SCOPE_IDENTITY();

    --------------------------------------------------
    -- COLOCA O SERVIÇO DENTRO DO PEDIDO
    --------------------------------------------------

    INSERT INTO ServicosPedidos
    (
        PedidoId,
        ServicoId,
        ProfissionalId,
        ValorServico,
        Observacao,
        Status
    )
    VALUES
    (
        @PedidoId,
        @ServicoId,
        15,
        220.00,
        'Primeira aula solicitada com profissional autônomo.',
        'Pendente'
    );

    COMMIT TRANSACTION;

    PRINT 'Pedido do profissional autônomo inserido com sucesso.';

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT ERROR_MESSAGE();

END CATCH;
GO