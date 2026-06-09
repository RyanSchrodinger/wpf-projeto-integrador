USE MusicStation
SELECT * FROM Usuarios
SELECT * FROM Administradores
SELECT * FROM Clientes
SELECT * FROM Profissionais
SELECT * FROM Empresas


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
(Id,Descricao, Rua, Numero, Cidade)
VALUES
(14, 'Guitarrista profissional', 'Rua A', '10', 'São Paulo'),
(15, 'Professora de piano', 'Rua B', '22', 'Osasco'),
(16, 'Baterista experiente', 'Rua C', '33', 'Barueri'),
(17, 'Cantora e compositora', 'Rua D', '44', 'Cotia'),
(18, 'Técnico de áudio', 'Rua E', '55', 'Suzano'),
(19, 'Violinista clássica', 'Rua F', '66', 'Mauá'),
(20, 'Produtor musical', 'Rua G', '77', 'Diadema'),
(21, 'Instrutora vocal', 'Rua H', '88', 'Taboão'),
(22, 'Baixista profissional', 'Rua I', '99', 'Carapicuíba'),
(23, 'DJ e produtora', 'Rua J', '100', 'São Paulo');


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

INSERT INTO TiposAcao(Nome)
VALUES ('Login')