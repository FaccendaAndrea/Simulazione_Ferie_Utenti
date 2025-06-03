import React, { useEffect, useState } from 'react';
import { useFormik } from 'formik';
import * as yup from 'yup';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Box
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { it } from 'date-fns/locale';
import { CategoriaPermesso, NuovaRichiestaPermesso } from '../types/permessi';
import * as api from '../services/api';

interface NuovaRichiestaDialogProps {
    open: boolean;
    onClose: () => void;
    onRichiestaCreata: () => void;
}

const validationSchema = yup.object({
    dataInizio: yup
        .date()
        .required('La data di inizio è obbligatoria')
        .min(new Date(), 'La data di inizio deve essere futura'),
    dataFine: yup
        .date()
        .required('La data di fine è obbligatoria')
        .min(yup.ref('dataInizio'), 'La data di fine deve essere successiva alla data di inizio'),
    motivazione: yup
        .string()
        .required('La motivazione è obbligatoria')
        .max(500, 'La motivazione non può superare i 500 caratteri'),
    categoriaID: yup
        .number()
        .required('La categoria è obbligatoria')
});

const NuovaRichiestaDialog: React.FC<NuovaRichiestaDialogProps> = ({
    open,
    onClose,
    onRichiestaCreata
}) => {
    const [categorie, setCategorie] = useState<CategoriaPermesso[]>([]);

    useEffect(() => {
        const loadCategorie = async () => {
            try {
                const data = await api.getCategorie();
                setCategorie(data);
            } catch (error) {
                console.error('Error loading categories:', error);
            }
        };

        if (open) {
            loadCategorie();
        }
    }, [open]);

    const formik = useFormik<NuovaRichiestaPermesso>({
        initialValues: {
            dataInizio: '',
            dataFine: '',
            motivazione: '',
            categoriaID: 0
        },
        validationSchema: validationSchema,
        onSubmit: async (values) => {
            try {
                await api.createRichiesta(values);
                onRichiestaCreata();
                formik.resetForm();
            } catch (error) {
                console.error('Error creating request:', error);
            }
        },
    });

    const handleClose = () => {
        formik.resetForm();
        onClose();
    };

    return (
        <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
            <DialogTitle>Nuova richiesta di permesso</DialogTitle>
            <form onSubmit={formik.handleSubmit}>
                <DialogContent>
                    <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={it}>
                        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                            <DatePicker
                                label="Data inizio"
                                value={formik.values.dataInizio ? new Date(formik.values.dataInizio) : null}
                                onChange={(date: Date | null) => formik.setFieldValue('dataInizio', date?.toISOString())}
                                disablePast
                                slotProps={{
                                    textField: {
                                        fullWidth: true,
                                        error: formik.touched.dataInizio && Boolean(formik.errors.dataInizio),
                                        helperText: formik.touched.dataInizio && formik.errors.dataInizio
                                    }
                                }}
                            />
                            <DatePicker
                                label="Data fine"
                                value={formik.values.dataFine ? new Date(formik.values.dataFine) : null}
                                onChange={(date: Date | null) => formik.setFieldValue('dataFine', date?.toISOString())}
                                disablePast
                                slotProps={{
                                    textField: {
                                        fullWidth: true,
                                        error: formik.touched.dataFine && Boolean(formik.errors.dataFine),
                                        helperText: formik.touched.dataFine && formik.errors.dataFine
                                    }
                                }}
                            />
                            <FormControl fullWidth>
                                <InputLabel id="categoria-label">Categoria</InputLabel>
                                <Select
                                    labelId="categoria-label"
                                    id="categoriaID"
                                    name="categoriaID"
                                    value={formik.values.categoriaID}
                                    onChange={formik.handleChange}
                                    error={formik.touched.categoriaID && Boolean(formik.errors.categoriaID)}
                                    label="Categoria"
                                >
                                    {categorie.map((categoria) => (
                                        <MenuItem key={categoria.categoriaID} value={categoria.categoriaID}>
                                            {categoria.descrizione}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            <TextField
                                fullWidth
                                id="motivazione"
                                name="motivazione"
                                label="Motivazione"
                                multiline
                                rows={4}
                                value={formik.values.motivazione}
                                onChange={formik.handleChange}
                                error={formik.touched.motivazione && Boolean(formik.errors.motivazione)}
                                helperText={formik.touched.motivazione && formik.errors.motivazione}
                            />
                        </Box>
                    </LocalizationProvider>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Annulla</Button>
                    <Button type="submit" variant="contained" color="primary">
                        Salva
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};

export default NuovaRichiestaDialog; 