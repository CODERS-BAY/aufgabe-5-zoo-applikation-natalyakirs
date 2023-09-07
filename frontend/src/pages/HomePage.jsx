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
                    backgroundImage: 'url("../../assets/backgroundImage.jpg")',
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
                        backgroundColor: 'rgba(241, 117, 181, 0.8)',
                        width: '70%',
                        maxWidth: '1344px',
                        transition: 'transform 0.3s',
                        '&:hover': {
                            transform: 'scale(1.05)',
                        }
                    }}
                >
                    <Typography variant="h5" gutterBottom>
                        Welcome to our zoo - the best zoo in Siberia!
                    </Typography>
                    <ServiceList/>
                </Paper>
            </Box>
        </Layout>
    );
};

export default HomePage;