import React, { useEffect, useState } from 'react';
import {
    Container,
    Paper,
    Typography,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Box,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Stack
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { it } from 'date-fns/locale';
import { CategoriaPermesso, Statistiche as StatisticheTipo } from '../types/permessi';
import * as api from '../services/api';

interface Utente {
    utenteID: number;
    nome: string;
    cognome: string;
    ruolo: string;
}

const Statistiche: React.FC = () => {
    const [statistiche, setStatistiche] = useState<StatisticheTipo[]>([]);
    const [utenti, setUtenti] = useState<Utente[]>([]);
    const [filtroAnno, setFiltroAnno] = useState<number>(new Date().getFullYear());
    const [filtroMese, setFiltroMese] = useState<number | null>(null);
    const [filtroUtenteID, setFiltroUtenteID] = useState<number | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const loadUtenti = async () => {
            try {
                const response = await api.getUtenti();
                setUtenti(response);
            } catch (error) {
                console.error('Error loading users:', error);
            }
        };
        loadUtenti();
    }, []);

    useEffect(() => {
        const loadStatistiche = async () => {
            setIsLoading(true);
            try {
                const data = await api.getStatistiche({
                    anno: filtroAnno,
                    mese: filtroMese || undefined,
                    utenteID: filtroUtenteID || undefined
                });
                setStatistiche(data);
            } catch (error) {
                console.error('Error loading statistics:', error);
            } finally {
                setIsLoading(false);
            }
        };

        loadStatistiche();
    }, [filtroAnno, filtroMese, filtroUtenteID]);

    const mesi = [
        { value: 1, label: 'Gennaio' },
        { value: 2, label: 'Febbraio' },
        { value: 3, label: 'Marzo' },
        { value: 4, label: 'Aprile' },
        { value: 5, label: 'Maggio' },
        { value: 6, label: 'Giugno' },
        { value: 7, label: 'Luglio' },
        { value: 8, label: 'Agosto' },
        { value: 9, label: 'Settembre' },
        { value: 10, label: 'Ottobre' },
        { value: 11, label: 'Novembre' },
        { value: 12, label: 'Dicembre' }
    ];

    if (isLoading) {
        return (
            <Container>
                <Typography>Caricamento...</Typography>
            </Container>
        );
    }

    return (
        <Container>
            <Box sx={{ my: 4 }}>
                <Typography variant="h4" component="h1" gutterBottom>
                    Statistiche permessi
                </Typography>
                <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2} sx={{ mb: 4 }}>
                    <Box sx={{ flex: 1 }}>
                        <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={it}>
                            <DatePicker
                                label="Anno"
                                views={['year']}
                                value={new Date(filtroAnno, 0)}
                                onChange={(date: Date | null) => date && setFiltroAnno(date.getFullYear())}
                                slotProps={{
                                    textField: { fullWidth: true }
                                }}
                            />
                        </LocalizationProvider>
                    </Box>
                    <Box sx={{ flex: 1 }}>
                        <FormControl fullWidth>
                            <InputLabel id="mese-label">Mese</InputLabel>
                            <Select
                                labelId="mese-label"
                                value={filtroMese || ''}
                                onChange={(e) => setFiltroMese(e.target.value ? Number(e.target.value) : null)}
                                label="Mese"
                            >
                                <MenuItem value="">Tutti</MenuItem>
                                {mesi.map((mese) => (
                                    <MenuItem key={mese.value} value={mese.value}>
                                        {mese.label}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </Box>
                    <Box sx={{ flex: 1 }}>
                        <FormControl fullWidth>
                            <InputLabel id="utente-label">Utente</InputLabel>
                            <Select
                                labelId="utente-label"
                                value={filtroUtenteID || ''}
                                onChange={(e) => setFiltroUtenteID(e.target.value ? Number(e.target.value) : null)}
                                label="Utente"
                            >
                                <MenuItem value="">Tutti</MenuItem>
                                {utenti.map((utente) => (
                                    <MenuItem key={utente.utenteID} value={utente.utenteID}>
                                        {utente.nome} {utente.cognome} ({utente.ruolo})
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </Box>
                </Stack>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Dipendente</TableCell>
                                <TableCell>Anno</TableCell>
                                <TableCell>Mese</TableCell>
                                <TableCell align="right">Giorni totali</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {statistiche.map((stat, index) => (
                                <TableRow key={index}>
                                    <TableCell>{stat.utenteNomeCompleto}</TableCell>
                                    <TableCell>{stat.anno}</TableCell>
                                    <TableCell>{mesi[stat.mese - 1].label}</TableCell>
                                    <TableCell align="right">{stat.giorniTotali}</TableCell>
                                </TableRow>
                            ))}
                            {statistiche.length === 0 && (
                                <TableRow>
                                    <TableCell colSpan={4} align="center">
                                        <Typography>Nessun dato disponibile per i filtri selezionati</Typography>
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
        </Container>
    );
};

export default Statistiche; 