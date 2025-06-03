import axios from 'axios';
import { LoginCredentials, RegisterData, User } from '../types/auth';
import { 
    CategoriaPermesso, 
    RichiestaPermesso, 
    NuovaRichiestaPermesso,
    ValutazionePermesso,
    Statistiche,
    StatisticheFilter
} from '../types/permessi';

const API_URL = 'http://localhost:5123/api';

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Interceptor per aggiungere il token JWT
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Auth
export const login = async (credentials: LoginCredentials): Promise<User> => {
    const response = await api.post('/auth/login', credentials);
    return response.data;
};

export const register = async (data: RegisterData): Promise<User> => {
    const response = await api.post('/auth/register', data);
    return response.data;
};

// Categorie
export const getCategorie = async (): Promise<CategoriaPermesso[]> => {
    const response = await api.get('/categorie');
    return response.data;
};

// Richieste
export const getRichieste = async (): Promise<RichiestaPermesso[]> => {
    const response = await api.get('/richieste');
    return response.data;
};

export const getRichiesta = async (id: number): Promise<RichiestaPermesso> => {
    const response = await api.get(`/richieste/${id}`);
    return response.data;
};

export const createRichiesta = async (richiesta: NuovaRichiestaPermesso): Promise<RichiestaPermesso> => {
    const response = await api.post('/richieste', richiesta);
    return response.data;
};

export const updateRichiesta = async (id: number, richiesta: NuovaRichiestaPermesso): Promise<void> => {
    await api.put(`/richieste/${id}`, richiesta);
};

export const deleteRichiesta = async (id: number): Promise<void> => {
    await api.delete(`/richieste/${id}`);
};

export const getRichiesteDaApprovare = async (): Promise<RichiestaPermesso[]> => {
    const response = await api.get('/richieste/da-approvare');
    return response.data;
};

export const valutaRichiesta = async (id: number, valutazione: ValutazionePermesso): Promise<void> => {
    await api.put(`/richieste/${id}/valuta`, valutazione);
};

// Statistiche
export const getStatistiche = async (filter: StatisticheFilter): Promise<Statistiche[]> => {
    const params = new URLSearchParams();
    if (filter.anno) params.append('anno', filter.anno.toString());
    if (filter.mese) params.append('mese', filter.mese.toString());
    if (filter.categoriaID) params.append('categoriaID', filter.categoriaID.toString());
    if (filter.utenteID) params.append('utenteID', filter.utenteID.toString());

    const response = await api.get('/statistiche', { params });
    return response.data;
};

export const getUtenti = async (): Promise<{ utenteID: number; nome: string; cognome: string; ruolo: string }[]> => {
    const response = await api.get('/utenti');
    return response.data;
}; 