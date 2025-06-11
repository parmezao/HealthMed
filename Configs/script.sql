
-- Tabela: usuarios
CREATE TABLE public.usuarios (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    senha_hash VARCHAR(1000) NOT NULL,
    role TEXT NOT NULL CHECK (role IN ('medico', 'paciente')),
    crm VARCHAR(50),
    cpf VARCHAR(11),
    email VARCHAR(255)
);

-- Tabela: medicos
CREATE TABLE public.medicos (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    crm VARCHAR(50) NOT NULL,
    especialidade VARCHAR(100) NOT NULL
);

-- Tabela: pacientes
CREATE TABLE public.pacientes (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cpf VARCHAR(11) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL 
);

-- Tabela: horarios_disponiveis
CREATE TABLE public.horarios_disponiveis (
    id SERIAL PRIMARY KEY,
    medico_id INT NOT NULL,
    data_hora TIMESTAMP WITH TIME ZONE NOT NULL,
    ocupado BOOLEAN DEFAULT FALSE,
    valor_consulta DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    FOREIGN KEY (medico_id) REFERENCES medicos(id) ON DELETE CASCADE
);
-- Tabela: consultas
CREATE TABLE public.consultas (
    id SERIAL PRIMARY KEY,
    cpf_paciente VARCHAR(11) NOT NULL,
    nome_paciente VARCHAR(255) NOT NULL,
    crm_medico VARCHAR(50) NOT NULL,
    data_hora TIMESTAMP WITH TIME ZONE NOT NULL,
    status VARCHAR(50) DEFAULT 'Pendente' CHECK (status IN ('Pendente', 'Aceita', 'Recusada', 'Cancelada')),
    justificativa VARCHAR(255)
);

-- Tabela: especialidades
CREATE TABLE public.especialidades (
    id SERIAL PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL UNIQUE,
    Categoria VARCHAR(50) NOT NULL
);

-- Inserts: Usuários
INSERT INTO public.usuarios (nome, senha_hash, role, crm, cpf, email) VALUES
('Dr. João Silva', '$2a$11$XakZu5DUwO0uBKbzxPDlr./huVR8xWzcmLyQAk8VSxTPKwG6fxpWS', 'medico', 'CRM123456', NULL, NULL),
('Maria Souza', '$2a$11$XakZu5DUwO0uBKbzxPDlr./huVR8xWzcmLyQAk8VSxTPKwG6fxpWS', 'paciente', NULL, '12345678901', 'maria@example.com'),
('Administrador', '$2a$11$XakZu5DUwO0uBKbzxPDlr./huVR8xWzcmLyQAk8VSxTPKwG6fxpWS', 'medico', 'CRMADMIN', NULL, NULL);

INSERT INTO public.pacientes (nome, cpf, email) VALUES
('Maria Souza', '12345678901', 'maria@example.com');



-- Inserts: Especialidades
INSERT INTO public.especialidades (Nome, Categoria) VALUES
('Clínica Geral', 'Clínica'),
('Cardiologia', 'Clínica'),
('Endocrinologia', 'Clínica'),
('Gastroenterologia', 'Clínica'),
('Geriatria', 'Clínica'),
('Neurologia', 'Clínica'),
('Pneumologia', 'Clínica'),
('Reumatologia', 'Clínica'),
('Pediatria', 'Pediátrica'),
('Ginecologia', 'Gineco/Obstetrícia'),
('Obstetrícia', 'Gineco/Obstetrícia'),
('Cirurgia Geral', 'Cirúrgica'),
('Cirurgia Plástica', 'Cirúrgica'),
('Ortopedia', 'Cirúrgica'),
('Urologia', 'Cirúrgica'),
('Otorrinolaringologia', 'Cirúrgica'),
('Radiologia', 'Diagnóstica'),
('Patologia Clínica', 'Diagnóstica'),
('Medicina Nuclear', 'Diagnóstica'),
('Psiquiatria', 'Clínica');



INSERT INTO public.medicos (nome, crm, especialidade) VALUES
('Dr. João Silva', 'CRM123456', 'Clínica Geral');
-- Índices
CREATE INDEX idx_medico_crm ON medicos(crm);
CREATE INDEX idx_usuario_login ON usuarios(crm, cpf, email);
CREATE INDEX idx_consulta_crm ON consultas(crm_medico);
CREATE INDEX idx_consulta_cpf ON consultas(cpf_paciente);
