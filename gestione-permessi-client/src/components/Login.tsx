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
    Link
} from '@mui/material';
import { useAuth } from '../contexts/AuthContext';

const validationSchema = yup.object({
    email: yup
        .string()
        .email('Inserisci un indirizzo email valido')
        .required('L\'email è obbligatoria'),
    password: yup
        .string()
        .required('La password è obbligatoria')
});

const Login: React.FC = () => {
    const navigate = useNavigate();
    const { login } = useAuth();

    const formik = useFormik({
        initialValues: {
            email: '',
            password: ''
        },
        validationSchema: validationSchema,
        onSubmit: async (values) => {
            try {
                await login(values);
                navigate('/dashboard');
            } catch (error) {
                console.error('Login error:', error);
                formik.setErrors({ email: 'Credenziali non valide' });
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
                        Accedi
                    </Typography>
                    <form onSubmit={formik.handleSubmit}>
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
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2 }}
                        >
                            Accedi
                        </Button>
                        <Box sx={{ textAlign: 'center' }}>
                            <Link href="/register" variant="body2">
                                Non hai un account? Registrati
                            </Link>
                        </Box>
                    </form>
                </Paper>
            </Box>
        </Container>
    );
};

export default Login; 