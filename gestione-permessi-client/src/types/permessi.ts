export interface CategoriaPermesso {
    categoriaID: number;
    descrizione: string;
}

export interface RichiestaPermesso {
    richiestaID: number;
    dataRichiesta: string;
    dataInizio: string;
    dataFine: string;
    motivazione: string;
    stato: 'In attesa' | 'Approvata' | 'Rifiutata';
    categoriaDescrizione: string;
    utenteNomeCompleto: string;
    dataValutazione?: string;
    utenteValutazioneNomeCompleto?: string;
}

export interface NuovaRichiestaPermesso {
    dataInizio: string;
    dataFine: string;
    motivazione: string;
    categoriaID: number;
}

export interface ValutazionePermesso {
    stato: 'Approvata' | 'Rifiutata';
}

export interface StatisticheFilter {
    anno?: number;
    mese?: number;
    categoriaID?: number;
    utenteID?: number;
}

export interface Statistiche {
    utenteNomeCompleto: string;
    anno: number;
    mese: number;
    giorniTotali: number;
    categoriaDescrizione: string;
} 