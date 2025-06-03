import React, { useEffect, useState } from 'react';
import {
    Container,
    Paper,
    Typography,
    Button,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Box,
    Chip
} from '@mui/material';
import { format } from 'date-fns';
import { it } from 'date-fns/locale';
import { useAuth } from '../contexts/AuthContext';
import { RichiestaPermesso } from '../types/permessi';
import * as api from '../services/api';
import NuovaRichiestaDialog from './NuovaRichiestaDialog';

const getStatoColor = (stato: string) => {
    switch (stato) {
        case 'Approvata':
            return 'success';
        case 'Rifiutata':
            return 'error';
        default:
            return 'warning';
    }
};

const Dashboard: React.FC = () => {
    const [richieste, setRichieste] = useState<RichiestaPermesso[]>([]);
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(true);

    const loadRichieste = async () => {
        try {
            const data = await api.getRichieste();
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

    const handleNuovaRichiesta = () => {
        setIsDialogOpen(true);
    };

    const handleCloseDialog = () => {
        setIsDialogOpen(false);
    };

    const handleRichiestaCreata = () => {
        loadRichieste();
        setIsDialogOpen(false);
    };

    const handleDeleteRichiesta = async (id: number) => {
        if (window.confirm('Sei sicuro di voler eliminare questa richiesta?')) {
            try {
                await api.deleteRichiesta(id);
                await loadRichieste();
            } catch (error) {
                console.error('Error deleting request:', error);
            }
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
                    Le tue richieste di permesso
                </Typography>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={handleNuovaRichiesta}
                    sx={{ mb: 3 }}
                >
                    Nuova richiesta
                </Button>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Data richiesta</TableCell>
                                <TableCell>Periodo</TableCell>
                                <TableCell>Categoria</TableCell>
                                <TableCell>Motivazione</TableCell>
                                <TableCell>Stato</TableCell>
                                <TableCell>Valutazione</TableCell>
                                <TableCell>Azioni</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {richieste.map((richiesta) => (
                                <TableRow key={richiesta.richiestaID}>
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
                                        <Chip
                                            label={richiesta.stato}
                                            color={getStatoColor(richiesta.stato)}
                                            size="small"
                                        />
                                    </TableCell>
                                    <TableCell>
                                        {richiesta.dataValutazione && (
                                            <>
                                                {format(new Date(richiesta.dataValutazione), 'dd/MM/yyyy', { locale: it })}
                                                <br />
                                                {richiesta.utenteValutazioneNomeCompleto}
                                            </>
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {richiesta.stato === 'In attesa' && (
                                            <Button
                                                variant="outlined"
                                                color="error"
                                                size="small"
                                                onClick={() => handleDeleteRichiesta(richiesta.richiestaID)}
                                            >
                                                Elimina
                                            </Button>
                                        )}
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
            <NuovaRichiestaDialog
                open={isDialogOpen}
                onClose={handleCloseDialog}
                onRichiestaCreata={handleRichiestaCreata}
            />
        </Container>
    );
};

export default Dashboard; 