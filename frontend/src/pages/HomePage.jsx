import {Box, Typography, Paper, useTheme} from '@mui/material';
import Layout from '../../Layout';
import ServiceList from '../../src/services/ServiceList';

const HomePage = () => {
    const theme = useTheme();

    return (
        <Layout>
            <Box
                sx={{
                    mt: 8,
                    mb: 4,
                    textAlign: 'center',
                    backgroundImage: 'url("../../assets/zoo-background.jpg")',
                    backgroundSize: 'cover',
                    backgroundRepeat: 'no-repeat',
                    backgroundPosition: 'center center',
                }}
            >
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    minHeight: '720px',
                    padding: theme.spacing(2),
                }}
            >
                <Paper
                    elevation={3}
                    sx={{
                        p: 4,
                        borderRadius: 2,
                        backgroundColor: 'rgba(255, 255, 255, 0.9)',
                        width: '70%',
                        maxWidth: '1344px',
                        transition: 'transform 0.3s',
                        '&:hover': {
                            transform: 'scale(1.05)',
                        }
                    }}
                >
                    <Typography variant="h5" gutterBottom>
                        Welcome to our zoo!
                    </Typography>
                    <ServiceList/>
                </Paper>
            </Box>
        </Layout>
    );
};

export default HomePage;