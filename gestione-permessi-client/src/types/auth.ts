export interface LoginCredentials {
    email: string;
    password: string;
}

export interface RegisterData extends LoginCredentials {
    nome: string;
    cognome: string;
    ruolo: 'Dipendente' | 'Responsabile';
}

export interface User {
    nome: string;
    cognome: string;
    email: string;
    ruolo: string;
    token: string;
}

export interface AuthContextType {
    user: User | null;
    login: (credentials: LoginCredentials) => Promise<void>;
    register: (data: RegisterData) => Promise<void>;
    logout: () => void;
    isAuthenticated: boolean;
} 