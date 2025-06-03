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
    Button,
    Box,
    ButtonGroup
} from '@mui/material';
import { format } from 'date-fns';
import { it } from 'date-fns/locale';
import { RichiestaPermesso, CategoriaPermesso } from '../types/permessi';
import * as api from '../services/api';
import NuovaRichiestaDialog from './NuovaRichiestaDialog';

const RichiesteDaApprovare: React.FC = () => {
    const [richieste, setRichieste] = useState<RichiestaPermesso[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
    const [richiestaDaModificare, setRichiestaDaModificare] = useState<RichiestaPermesso | null>(null);
    const [categorie, setCategorie] = useState<CategoriaPermesso[]>([]);

    const mapRichiesta = (r: any): RichiestaPermesso => ({
        richiestaID: r.richiestaID ?? r.richiestaID ?? r.id,
        dataRichiesta: r.dataRichiesta ?? r.dataRichiesta,
        dataInizio: r.dataInizio ?? r.dataInizio,
        dataFine: r.dataFine ?? r.dataFine,
        motivazione: r.motivazione ?? r.motivazione,
        stato: r.stato ?? r.stato,
        categoriaDescrizione: r.categoriaDescrizione || (categorie.find(c => c.categoriaID === (r.categoriaID ?? r.categoriaId))?.descrizione ?? ''),
        utenteNomeCompleto: r.utenteNomeCompleto || (r.utente ? `${r.utente.nome} ${r.utente.cognome}` : ''),
        dataValutazione: r.dataValutazione,
        utenteValutazioneNomeCompleto: r.utenteValutazioneNomeCompleto || (r.utenteValutazione ? `${r.utenteValutazione.nome} ${r.utenteValutazione.cognome}` : undefined)
    });

    const loadRichieste = async () => {
        try {
            const data = await api.getRichiesteDaApprovare();
            setRichieste(Array.isArray(data) ? data.map(mapRichiesta) : []);
        } catch (error) {
            console.error('Error loading requests:', error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        loadRichieste();
        // Carica le categorie per il mapping
        const loadCategorie = async () => {
            try {
                const data = await api.getCategorie();
                setCategorie(data);
            } catch (error) {
                console.error('Error loading categories:', error);
            }
        };
        loadCategorie();
    }, []);

    const handleValutazione = async (id: number, stato: 'Approvata' | 'Rifiutata') => {
        try {
            await api.valutaRichiesta(id, { stato });
            await loadRichieste();
        } catch (error) {
            console.error('Error evaluating request:', error);
        }
    };

    const handleOpenEditDialog = (richiesta: RichiestaPermesso) => {
        setRichiestaDaModificare(richiesta);
        setIsEditDialogOpen(true);
    };

    const handleCloseEditDialog = () => {
        setIsEditDialogOpen(false);
        setRichiestaDaModificare(null);
    };

    const handleRichiestaAggiornata = async () => {
        await loadRichieste();
        setIsEditDialogOpen(false);
        setRichiestaDaModificare(null);
    };

    const categorieToId = (descrizione: string): number => {
        const cat = categorie.find(c => c.descrizione === descrizione);
        return cat ? cat.categoriaID : 0;
    };

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
                    Richieste da approvare
                </Typography>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Dipendente</TableCell>
                                <TableCell>Data richiesta</TableCell>
                                <TableCell>Periodo</TableCell>
                                <TableCell>Categoria</TableCell>
                                <TableCell>Motivazione</TableCell>
                                <TableCell>Azioni</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {richieste.map((richiesta) => (
                                <TableRow key={richiesta.richiestaID}>
                                    <TableCell>{richiesta.utenteNomeCompleto}</TableCell>
                                    <TableCell>
                                        {format(new Date(richiesta.dataRichiesta), 'dd/MM/yyyy', { locale: it })}
                                    </TableCell>
                                    <TableCell>
                                        {format(new Date(richiesta.dataInizio), 'dd/MM/yyyy', { locale: it })}
                                        {' - '}
                                        {format(new Date(richiesta.dataFine), 'dd/MM/yyyy', { locale: it })}
                                    </TableCell>
                                    <TableCell>{richiesta.categoriaDescrizione}</TableCell>
                                    <TableCell>{richiesta.motivazione}</TableCell>
                                    <TableCell>
                                        <ButtonGroup>
                                            <Button
                                                variant="contained"
                                                color="success"
                                                onClick={() => handleValutazione(richiesta.richiestaID, 'Approvata')}
                                            >
                                                Approva
                                            </Button>
                                            <Button
                                                variant="contained"
                                                color="error"
                                                onClick={() => handleValutazione(richiesta.richiestaID, 'Rifiutata')}
                                            >
                                                Rifiuta
                                            </Button>
                                            <Button
                                                variant="outlined"
                                                color="primary"
                                                onClick={() => handleOpenEditDialog(richiesta)}
                                                disabled={richiesta.stato !== 'In attesa'}
                                            >
                                                Modifica
                                            </Button>
                                        </ButtonGroup>
                                    </TableCell>
                                </TableRow>
                            ))}
                            {richieste.length === 0 && (
                                <TableRow>
                                    <TableCell colSpan={6} align="center">
                                        <Typography>Non ci sono richieste da approvare</Typography>
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
            <NuovaRichiestaDialog
                open={isEditDialogOpen}
                onClose={handleCloseEditDialog}
                onRichiestaCreata={() => {}}
                richiestaDaModificare={richiestaDaModificare ? {
                    richiestaID: richiestaDaModificare.richiestaID,
                    dataInizio: richiestaDaModificare.dataInizio,
                    dataFine: richiestaDaModificare.dataFine,
                    motivazione: richiestaDaModificare.motivazione,
                    categoriaID: richiestaDaModificare.categoriaDescrizione ? categorieToId(richiestaDaModificare.categoriaDescrizione) : 0
                } : undefined}
                onRichiestaAggiornata={handleRichiestaAggiornata}
            />
        </Container>
    );
};

export default RichiesteDaApprovare; 