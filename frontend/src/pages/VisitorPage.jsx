import { useState, useEffect, useCallback } from 'react';
import {
    Container,
    Grid,
    TextField,
    IconButton,
    CircularProgress,
    Typography,
} from '@mui/material';
import { Box } from '@mui/system';
import SearchIcon from '@mui/icons-material/Search';
import Layout from "../../Layout.jsx";
import AnimalCard from "../components/AnimalCard.jsx";

const API_ENDPOINT = 'http://localhost:5207/api';

function VisitorPage() {
    const [animals, setAnimals] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [loading, setLoading] = useState(false);
    const [animalImages, setAnimalImages] = useState({});
    const [error, setError] = useState(null);

    const fetchAnimals = useCallback(async (species = '') => {
        setLoading(true);
        const endpoint = species
            ? `${API_ENDPOINT}/Visitor/animals/${species}`
            : `${API_ENDPOINT}/visitor/animals`;
        try {
            const response = await fetch(endpoint);
            if (!response.ok) {
                throw new Error(`API request failed with status ${response.status}`);
            }
            const data = await response.json();
            setAnimals(Array.isArray(data) ? data : [data]);
        } catch (error) {
            setError('Error fetching data. Please try again.');
            console.error('Error fetching data:', error);
        }
        setLoading(false);
    }, []);

    useEffect(() => {
        fetchAnimals();
    }, [fetchAnimals]);

    const loadImageForAnimal = async (species) => {
        try {
            const image = await import(`../assets/${species}.jpg`);
            setAnimalImages((prevImages) => ({...prevImages, [species]: image.default}));
        } catch (error) {
            console.error('Error loading image:', error);
        }
    };

    useEffect(() => {
        animals.forEach((animal) => {
            loadImageForAnimal(animal.species);
        });
    }, [animals]);

    const handleSearch = useCallback(() => {
        setError(null);
        fetchAnimals(searchTerm);
    }, [fetchAnimals, searchTerm]);

    const handleKeyPress = (event) => {
        if (event.key === 'Enter') {
            handleSearch();
        }
    };

    return (
        <Container>
            <Layout>
                <Box sx={{mt: 4, mb: 4, textAlign: 'center'}}>
                    <Typography variant="h4" gutterBottom>
                        Animals in the zoo
                    </Typography>
                </Box>
                <Box sx={{mb: 4, display: 'flex', alignItems: 'center'}}>
                    <TextField
                        label="Search by species"
                        variant="outlined"
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        onKeyUp={handleKeyPress}
                        fullWidth
                        sx={{mr: 2}}
                    />
                    <IconButton onClick={handleSearch} color="primary">
                        <SearchIcon/>
                    </IconButton>
                </Box>
                {loading ? (
                    <Box sx={{display: 'flex', justifyContent: 'center', mt: 4}}>
                        <CircularProgress/>
                    </Box>
                ) : error ? (
                    <Typography variant="h6" color="error" textAlign="center">{error}</Typography>
                ) : (
                    <Grid container spacing={3}>
                        {animals.map((animal) => (
                            <Grid item xs={12} sm={6} md={4} key={animal.id}>
                                <AnimalCard animal={animal} animalImages={animalImages}/>
                            </Grid>
                        ))}
                    </Grid>
                )}
            </Layout>
        </Container>
    );
}

export default VisitorPage;