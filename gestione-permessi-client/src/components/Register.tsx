import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import * as yup from 'yup';
import {
    Container,
    Paper,
    Typography,
    TextField,
    Button,
    Box,
    Link,
    FormControl,
    InputLabel,
    Select,
    MenuItem
} from '@mui/material';
import { useAuth } from '../contexts/AuthContext';
import { RegisterData } from '../types/auth';

const validationSchema = yup.object({
    nome: yup
        .string()
        .required('Il nome è obbligatorio')
        .max(50, 'Il nome non può superare i 50 caratteri'),
    cognome: yup
        .string()
        .required('Il cognome è obbligatorio')
        .max(50, 'Il cognome non può superare i 50 caratteri'),
    email: yup
        .string()
        .email('Inserisci un indirizzo email valido')
        .required('L\'email è obbligatoria')
        .max(100, 'L\'email non può superare i 100 caratteri'),
    password: yup
        .string()
        .required('La password è obbligatoria')
        .min(8, 'La password deve essere di almeno 8 caratteri')
        .matches(/[a-zA-Z]/, 'La password deve contenere almeno una lettera')
        .matches(/[0-9]/, 'La password deve contenere almeno un numero'),
    ruolo: yup
        .string()
        .oneOf(['Dipendente', 'Responsabile'] as const, 'Seleziona un ruolo valido')
        .required('Il ruolo è obbligatorio')
});

const Register: React.FC = () => {
    const navigate = useNavigate();
    const { register } = useAuth();

    const formik = useFormik<RegisterData>({
        initialValues: {
            nome: '',
            cognome: '',
            email: '',
            password: '',
            ruolo: 'Dipendente'
        },
        validationSchema: validationSchema,
        onSubmit: async (values) => {
            try {
                await register(values);
                navigate('/dashboard');
            } catch (error) {
                console.error('Registration error:', error);
                formik.setErrors({ email: 'Email già registrata' });
            }
        },
    });

    return (
        <Container component="main" maxWidth="xs">
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Paper elevation={3} sx={{ p: 4, width: '100%' }}>
                    <Typography component="h1" variant="h5" align="center" gutterBottom>
                        Registrazione
                    </Typography>
                    <form onSubmit={formik.handleSubmit}>
                        <TextField
                            fullWidth
                            id="nome"
                            name="nome"
                            label="Nome"
                            value={formik.values.nome}
                            onChange={formik.handleChange}
                            error={formik.touched.nome && Boolean(formik.errors.nome)}
                            helperText={formik.touched.nome && formik.errors.nome}
                            margin="normal"
                        />
                        <TextField
                            fullWidth
                            id="cognome"
                            name="cognome"
                            label="Cognome"
                            value={formik.values.cognome}
                            onChange={formik.handleChange}
                            error={formik.touched.cognome && Boolean(formik.errors.cognome)}
                            helperText={formik.touched.cognome && formik.errors.cognome}
                            margin="normal"
                        />
                        <TextField
                            fullWidth
                            id="email"
                            name="email"
                            label="Email"
                            value={formik.values.email}
                            onChange={formik.handleChange}
                            error={formik.touched.email && Boolean(formik.errors.email)}
                            helperText={formik.touched.email && formik.errors.email}
                            margin="normal"
                        />
                        <TextField
                            fullWidth
                            id="password"
                            name="password"
                            label="Password"
                            type="password"
                            value={formik.values.password}
                            onChange={formik.handleChange}
                            error={formik.touched.password && Boolean(formik.errors.password)}
                            helperText={formik.touched.password && formik.errors.password}
                            margin="normal"
                        />
                        <FormControl fullWidth margin="normal">
                            <InputLabel id="ruolo-label">Ruolo</InputLabel>
                            <Select
                                labelId="ruolo-label"
                                id="ruolo"
                                name="ruolo"
                                value={formik.values.ruolo}
                                onChange={formik.handleChange}
                                error={formik.touched.ruolo && Boolean(formik.errors.ruolo)}
                                label="Ruolo"
                            >
                                <MenuItem value="Dipendente">Dipendente</MenuItem>
                                <MenuItem value="Responsabile">Responsabile</MenuItem>
                            </Select>
                        </FormControl>
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Registrati
                        </Button>
                        <Box sx={{ textAlign: 'center' }}>
                            <Link href="/login" variant="body2">
                                Hai già un account? Accedi
                            </Link>
                        </Box>
                    </form>
                </Paper>
            </Box>
        </Container>
    );
};

export default Register; 