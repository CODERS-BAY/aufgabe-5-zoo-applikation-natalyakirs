import React, {useState, useEffect, useCallback} from 'react';
import { Container,Grid,Card,CardMedia, CardContent,Typography,TextField, CircularProgress,Button,IconButton,Tooltip,} from '@mui/material';
import {Box} from '@mui/system';
import SearchIcon from '@mui/icons-material/Search';
import {fetchAssignedAnimals, updateAnimal} from '../../api';

function AnimalKeeperPage() {
    const [animalkeeperId, setAnimalkeeperId] = useState('');
    const [assignedAnimals, setAssignedAnimals] = useState([]);
    const [loading, setLoading] = useState(false);
    const [animalImages, setAnimalImages] = useState({});

    const fetchAssignedAnimalsDebounced = useCallback(async (id) => {
        setLoading(true);
        try {
            const data = await fetchAssignedAnimals(id);
            setAssignedAnimals(data);
        } catch (error) {
            console.error('Error fetching assigned animals:', error);
        }
        setLoading(false);
    }, []);

    useEffect(() => {
        const loadImage = async (species) => {
            try {
                const image = await import(`../assets/${species}.jpg`);
                setAnimalImages((prevImages) => ({...prevImages, [species]: image.default}));
            } catch (error) {
                console.error('Error loading image:', error);
            }
        };

        assignedAnimals.forEach((animal) => {
            loadImage(animal.species);
        });
    }, [assignedAnimals]);

    const handleUpdateAnimal = async (animalId) => {
        const updatedData = {
            species: document.getElementById(`species-${animalId}`).value,
            food: document.getElementById(`food-${animalId}`).value,
            enclosureId: parseInt(document.getElementById(`enclosureId-${animalId}`).value),
        };
        try {
            await updateAnimal(animalId, updatedData);
            setAssignedAnimals((prevAnimals) =>
                prevAnimals.map((animal) => (animal.id === animalId ? {...animal, ...updatedData} : animal))
            );
        } catch (error) {
            console.error('Error updating animal:', error);
        }
    };

    return (
        <Container>
            <Box sx={{mt: 4, mb: 4, textAlign: 'center'}}>
                <Typography variant="h4" gutterBottom>
                    Animal keeper
                </Typography>
            </Box>
            <Box sx={{mb: 4, display: 'flex', alignItems: 'center'}}>
                <TextField
                    label="Animalkeeper-ID"
                    variant="outlined"
                    value={animalkeeperId}
                    onChange={(e) => setAnimalkeeperId(e.target.value)}
                    onKeyPress={(e) => {
                        if (e.key === 'Enter') {
                            fetchAssignedAnimalsDebounced(animalkeeperId);
                            e.preventDefault();
                        }
                    }}
                    fullWidth
                    sx={{mr: 2}}
                />
                <IconButton onClick={() => fetchAssignedAnimalsDebounced(animalkeeperId)} color="primary">
                    <SearchIcon/>
                </IconButton>
            </Box>
            {loading ? (
                <Box sx={{display: 'flex', justifyContent: 'center', mt: 4}}>
                    <CircularProgress/>
                </Box>
            ) : (
                <Grid container spacing={3}>
                    {assignedAnimals.map((animal) => (
                        <Grid item xs={12} sm={6} md={4} key={animal.id}>
                            <Card sx={{height: '100%', display: 'flex', flexDirection: 'column'}}>
                                <CardMedia component="img" alt={animal.species} height="140"
                                           image={animalImages[animal.species]}/>
                                <CardContent sx={{flexGrow: 1}}>
                                    <Tooltip title="Species of animals">
                                        <TextField
                                            id={`species-${animal.id}`}
                                            label="Species"
                                            defaultValue={animal.species}
                                            variant="outlined"
                                            fullWidth
                                            margin="normal"
                                        />
                                    </Tooltip>
                                    <Tooltip title="Food of animals">
                                        <TextField
                                            id={`food-${animal.id}`}
                                            label="Food"
                                            defaultValue={animal.food}
                                            variant="outlined"
                                            fullWidth
                                            margin="normal"
                                            type="number"
                                        />
                                    </Tooltip>
                                    <Button variant="contained" color="primary" sx={{mt: 2, width: '100%'}}
                                            onClick={() => handleUpdateAnimal(animal.id)}>
                                        Update
                                    </Button>
                                </CardContent>
                            </Card>
                        </Grid>
                    ))}
                </Grid>
            )}
        </Container>
    );
}

export default AnimalKeeperPage;