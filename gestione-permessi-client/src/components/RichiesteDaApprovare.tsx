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
import { RichiestaPermesso } from '../types/permessi';
import * as api from '../services/api';

const RichiesteDaApprovare: React.FC = () => {
    const [richieste, setRichieste] = useState<RichiestaPermesso[]>([]);
    const [isLoading, setIsLoading] = useState(true);

    const loadRichieste = async () => {
        try {
            const data = await api.getRichiesteDaApprovare();
            setRichieste(data);
        } catch (error) {
            console.error('Error loading requests:', error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        loadRichieste();
    }, []);

    const handleValutazione = async (id: number, stato: 'Approvata' | 'Rifiutata') => {
        try {
            await api.valutaRichiesta(id, { stato });
            await loadRichieste();
        } catch (error) {
            console.error('Error evaluating request:', error);
        }
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
        </Container>
    );
};

export default RichiesteDaApprovare; 